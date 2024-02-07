namespace RedFox.PasswordStorage.Infrastructure.Services;

public interface IUserInfoService
{
    public Task<string> GetInfo(string accessToken, string refreshToken, string login);

    public Task<bool> UpdateInfo(string accessToken, string refreshToken, string login, string list);

    public Task<string> GetNewAccessToken();

    public Task<string> GetNewRefreshToken();
}