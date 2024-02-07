using RedFox.PasswordStorage.Domain.Entities;
using RedFox.PasswordStorage.Domain.Repositories;
using RedFox.PasswordStorage.Infrastructure.Helpers;
using RedFox.PasswordStorage.Infrastructure.Models;

namespace RedFox.PasswordStorage.Infrastructure.Services;

public sealed class RegisterService : IRegisterService
{
    public async Task<bool> Register(RegisterModel registerModel)
    {
        UserDataBaseRepository db = new ();
        var loginExists = await db.GetUserByLogin(registerModel.Login);
        
        if (loginExists.Count != 0)
            return false;
        
        await db.CreateUser(new UserEntity
        {
            Id  = Guid.NewGuid(),
            Login = registerModel.Login,
            Password = HashPasswordHelper.HashPasswordToSha256(registerModel.Password)
        });
        
        return true;
    }
}