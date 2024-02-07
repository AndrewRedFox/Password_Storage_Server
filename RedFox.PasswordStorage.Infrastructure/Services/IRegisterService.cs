using RedFox.PasswordStorage.Infrastructure.Models;

namespace RedFox.PasswordStorage.Infrastructure.Services;

public interface IRegisterService
{
    public Task<bool> Register(RegisterModel registerModel);
}