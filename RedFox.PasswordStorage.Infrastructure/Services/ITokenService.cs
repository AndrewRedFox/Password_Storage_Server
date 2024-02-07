using RedFox.PasswordStorage.Domain.Entities;

namespace RedFox.PasswordStorage.Infrastructure.Services;

public interface ITokenService
{
    public string CreateAccessToken(UserEntity entity);

    public string CreateRefreshToken(UserEntity entity);

    public bool ValidateToken(string token, string validIssuer, string dbToken);
}