using RedFox.PasswordStorage.Domain.Entities;
namespace RedFox.PasswordStorage.Infrastructure.Services;

public interface IAuthService
{
    public Task<bool> Auth(string login, string password);

    public bool IsAuth(UserEntity user, string password);

    public UserEntity GetUserModel();

    public void SetAccessToken(string accessToken);

    public void SetRefreshToken(string refreshToken);
}