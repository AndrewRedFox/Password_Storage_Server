using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RedFox.PasswordStorage.Domain.Entities;
using RedFox.PasswordStorage.Infrastructure.Secure;

namespace RedFox.PasswordStorage.Infrastructure.Services;

public sealed class TokenService : ITokenService
{
    public string CreateAccessToken(UserEntity entity)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(SecretKey.secretKey);

        var descriptor = new SecurityTokenDescriptor
        {
            Audience = "client",
            Issuer = entity.Login,
            Expires = DateTime.Now.AddMinutes(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
        };

        SecurityToken token = tokenHandler.CreateToken(descriptor);
        string accessToken = tokenHandler.WriteToken(token);

        return accessToken;
    }
    
    public string CreateRefreshToken(UserEntity entity)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(SecretKey.secretKey);

        var descriptor = new SecurityTokenDescriptor
        {
            Audience = "client",
            Issuer = entity.Login,
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
        };

        SecurityToken token = tokenHandler.CreateToken(descriptor);
        string refreshToken = tokenHandler.WriteToken(token);

        return refreshToken;
    }

    public bool ValidateToken(string Token, string validIssuer, string dbToken)
    {
        if (!Equals(Token, dbToken))
            return false;
        
        TokenValidationParameters validationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = validIssuer,
            ValidAudience = "client",
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey.secretKey)),
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(Token, validationParameters, out validatedToken);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    
}