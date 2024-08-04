using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotNetAPILearn.Data;
using DotNetAPILearn.Dtos;
using DotNetAPILearn.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace DotNetAPILearn.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IConfiguration _config) : ControllerBase
    {
        private readonly DataContextDapper _dapper = new(_config);
        private readonly AuthHelper _authHelper = new(_config);

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            string sql = $@"
            SELECT PasswordHash,PasswordSalt
            FROM TutorialAppSchema.Auth
            WHERE Email = '{userForLoginDto.Email}'";

            UserForLoginConfirmationDto userForLoginConfirmationDto = _dapper.LoadDataSingle<UserForLoginConfirmationDto>(sql);

            byte[] passwordHash = _authHelper.GeneratePasswordHash(userForLoginDto.Password, userForLoginConfirmationDto.PasswordSalt);


            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != userForLoginConfirmationDto.PasswordHash[i])
                    return StatusCode(401, "Incorrect Password");
            }

            int userId = GetUserIdByEmail(userForLoginDto.Email);

            string token = _authHelper.CreateToken(userId);

            return Ok(new Dictionary<string, string> {
                {"token", _authHelper.CreateToken(userId)}
            });
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            if (userForRegisterDto.Password != userForRegisterDto.PasswordConfirm)
                throw new Exception("Password do not match");

            if (IsUserEmailExist(userForRegisterDto.Email))
                throw new Exception("User email already exist");



            byte[] passwordSalt = _authHelper.GeneratePasswordSalt();

            byte[] passwordHash = _authHelper.GeneratePasswordHash(userForRegisterDto.Password, passwordSalt);

            InsertUserAuth(email: userForRegisterDto.Email, passwordSalt: passwordSalt, passwordHash: passwordHash);
            AddUser(userForRegisterDto);
            return Ok();
        }

        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            int userId = int.Parse(User.FindFirst("userId")?.Value ?? "");
            return Ok(new Dictionary<string, string> {
                {"token", _authHelper.CreateToken(userId)}
            });
        }

        private void AddUser(UserForRegisterDto user)
        {
            string sql = $@"
            INSERT INTO TutorialAppSchema.Users (
                    FirstName
                    ,LastName
                    ,Email
                    ,Gender
                    ,Active)
            VALUES (
                    '{user.FirstName}'
                    ,'{user.LastName}'
                    ,'{user.Email}'
                    ,'{user.Gender}'
                    ,1
                );";
            if (!_dapper.ExecuteSql(sql))
                throw new Exception("Failed to add user");

        }

        bool IsUserEmailExist(string Email)
        {
            string sqlCheckIfEmailExist = $@"
                SELECT * 
                FROM TutorialAppSchema.Auth
                WHERE Email = '{Email}'";

            IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckIfEmailExist);

            return existingUsers.Any();

        }



        void InsertUserAuth(string email, byte[] passwordSalt, byte[] passwordHash)
        {
            string sql = $@"
            INSERT INTO TutorialAppSchema.Auth(Email ,PasswordHash ,PasswordSalt)
            VALUES('{email}',@PasswordHash,@PasswordSalt)";

            List<SqlParameter> parameters = [];
            SqlParameter passwordSaltParameter = new("PasswordSalt", SqlDbType.VarBinary)
            {
                Value = passwordSalt
            };
            SqlParameter passwordHashParameter = new("PasswordHash", SqlDbType.VarBinary)
            {
                Value = passwordHash
            };
            parameters.Add(passwordSaltParameter);
            parameters.Add(passwordHashParameter);

            if (!_dapper.ExecuteSqlWithParameters(sql, parameters))
                throw new Exception("Failed to register user");

        }

        private int GetUserIdByEmail(string email)
        {
            string userIdSql = @"
                SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" +
              email + "'";
            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return userId;

        }


    }

}