using System.Data;
using System.Security.Cryptography;
using System.Text;
using DotNetAPILearn.Data;
using DotNetAPILearn.Dtos;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotNetAPILearn.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IConfiguration _config) : ControllerBase
    {
        private readonly DataContextDapper _dapper = new(_config);


        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            string sql = $@"
            SELECT PasswordHash,PasswordSalt
            FROM TutorialAppSchema.Auth
            WHERE Email = '{userForLoginDto.Email}'";

            UserForLoginConfirmationDto userForLoginConfirmationDto = _dapper.LoadDataSingle<UserForLoginConfirmationDto>(sql);

            byte[] passwordHash = GeneratePasswordHash(userForLoginDto.Password, userForLoginConfirmationDto.PasswordSalt);


            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != userForLoginConfirmationDto.PasswordHash[i])
                    return StatusCode(401, "Incorrect Password");
            }


            return Ok();
        }
        [HttpPost("Register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            if (userForRegisterDto.Password != userForRegisterDto.PasswordConfirm)
                throw new Exception("Password do not match");

            if (IsUserEmailExist(userForRegisterDto.Email))
                throw new Exception("User email already exist");



            byte[] passwordSalt = GeneratePasswordSalt();

            byte[] passwordHash = GeneratePasswordHash(userForRegisterDto.Password, passwordSalt);

            InsertUserAuth(email: userForRegisterDto.Email, passwordSalt: passwordSalt, passwordHash: passwordHash);
            AddUser(userForRegisterDto);
            return Ok();
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

        byte[] GeneratePasswordSalt()
        {

            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }
            return passwordSalt;
        }

        byte[] GeneratePasswordHash(string password, byte[] passwordSalt)
        {
            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value +
                Convert.ToBase64String(passwordSalt);
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            );
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
    }



}