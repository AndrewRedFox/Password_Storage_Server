namespace RedFox.PasswordStorage.Infrastructure.Models;

public sealed class RegisterModel
{
    public RegisterModel(string login, string password)
    {
        Login = login;
        Password = password;
    }
    public string Login { get; set; }
    
    public string Password { get; set; }
}