using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace DotNetAPILearn.Helpers;

public class AuthHelper(IConfiguration _config)
{

    public string CreateToken(int userId)
    {
        Claim[] claims = [
            new("userId", userId.ToString())
        ];

        string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;

        SymmetricSecurityKey tokenKey = new(
                Encoding.UTF8.GetBytes(
                    tokenKeyString ?? ""
                )
            );

        SigningCredentials credentials = new SigningCredentials(
                tokenKey,
                SecurityAlgorithms.HmacSha512Signature
            );

        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddDays(1)
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken token = tokenHandler.CreateToken(descriptor);

        return tokenHandler.WriteToken(token);

    }

    public byte[] GeneratePasswordSalt()
    {

        byte[] passwordSalt = new byte[128 / 8];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }
        return passwordSalt;
    }

    public byte[] GeneratePasswordHash(string password, byte[] passwordSalt)
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

}

