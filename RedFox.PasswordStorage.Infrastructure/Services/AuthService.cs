using Microsoft.IdentityModel.Tokens;
using RedFox.PasswordStorage.Domain.Entities;
using RedFox.PasswordStorage.Domain.Repositories;
using RedFox.PasswordStorage.Infrastructure.Helpers;

namespace RedFox.PasswordStorage.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserDataBaseRepository db = new();
    private UserEntity? _entity;
    
    public async Task<bool> Auth (string login, string password)
    {
        var user = await db.GetUserByLogin(login);

        if (user.IsNullOrEmpty()) return false;

        if(IsAuth(user[0], password))
        {
            _entity = user[0];
            TokenService tokenService = new();
            _entity.AccessToken = tokenService.CreateAccessToken(_entity);
            _entity.RefreshToken = tokenService.CreateRefreshToken(_entity);
            await db.UpdateUser(_entity);
            return true;
        }
        return false;
    }
    
    public bool IsAuth(UserEntity user, string password)
    {
        if (Equals(HashPasswordHelper.HashPasswordToSha256(password), user.Password))
            return true;
        
        return false;
    }
    
    public UserEntity GetUserModel() { return _entity; }
    
    public async void SetAccessToken(string accessToken)
    {
        _entity.AccessToken = accessToken;
        await db.UpdateUser(_entity);
    }

    public async void SetRefreshToken(string refreshToken)
    {
        _entity.RefreshToken = refreshToken;
        await db.UpdateUser(_entity);
    }

    public string GetAccessToken()
    {
        return _entity.AccessToken;
    }
    
    public string GetRefreshToken()
    {
        return _entity.RefreshToken;
    }
}