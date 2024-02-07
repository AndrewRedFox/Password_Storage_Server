using Microsoft.IdentityModel.Tokens;
using RedFox.PasswordStorage.Domain.Entities;
using RedFox.PasswordStorage.Domain.Repositories;

namespace RedFox.PasswordStorage.Infrastructure.Services;

public sealed class UserInfoService : IUserInfoService
{
    private readonly UserDataBaseRepository db = new();
    private UserEntity? _entity;
    
    public async Task<string> GetInfo(string accessToken, string refreshToken, string login)
    {
        var users = await db.GetUserByLogin(login);
        TokenService tokenService = new TokenService();
        if (users.IsNullOrEmpty())
        {
            return "Non";
        }
        _entity = users[0];

        if (tokenService.ValidateToken(accessToken, _entity.Login, _entity.AccessToken))
            return _getListOfPassword();
        
        if(tokenService.ValidateToken(refreshToken, _entity.Login, _entity.RefreshToken))
        {
            _entity.AccessToken = tokenService.CreateAccessToken(_entity);
            _entity.RefreshToken = tokenService.CreateRefreshToken(_entity);
            await db.UpdateUser(_entity);

            return _getListOfPassword();
        }
        return "Non";
    }
    
    public async Task<bool> UpdateInfo(string accessToken, string refreshToken, string login, string list)
    {
        var users = await db.GetUserByLogin(login);
        TokenService tokenService = new TokenService();
        _entity = users[0]; 

        if (tokenService.ValidateToken(accessToken, _entity.Login, _entity.AccessToken))
        {
            _entity.UserInfo = _UpdateList(list);
            await db.UpdateUser(_entity);
        }
        else if (tokenService.ValidateToken(refreshToken, _entity.Login, _entity.RefreshToken))
        {
            _entity.AccessToken = tokenService.CreateAccessToken(_entity);
            _entity.RefreshToken = tokenService.CreateRefreshToken(_entity);
            _entity.UserInfo = _UpdateList(list);
            await db.UpdateUser(_entity);
        }
        return true;
    }
    
    private string _getListOfPassword()
    {
        string list = "";
        if(_entity.UserInfo.IsNullOrEmpty()) return list;
        foreach (var item in _entity.UserInfo)
        {
            list += "`" + item.ApplicationName + "~" + item.Login + "~" + item.Password;
        }
        return list;
    }
    
    private List<UserInfoEntity> _UpdateList(string list)
    {
        List<UserInfoEntity> listOfInfo = new();
        var l = list.Split('`').ToList();//.
        l.RemoveAll(s => s == "");

        foreach(var pair in l)
        {
            UserInfoEntity userInfo = new();
            string[] words = pair.Split(new char[] { '~' });///
            userInfo.ApplicationName = words[0];
            userInfo.Login = words[1];
            userInfo.Password = words[2];
            listOfInfo.Add(userInfo);
        }
        return listOfInfo;
    }
    
    public async Task<string> GetNewAccessToken()
    {
        return _entity.AccessToken;
    }

    public async Task<string> GetNewRefreshToken()
    {
        return _entity.RefreshToken;
    }
}