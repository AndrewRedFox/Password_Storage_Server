namespace RedFox.PasswordStorage.Domain.Entities;

public sealed class UserInfoEntity : Entity
{
    public string Password { get; set; }
    
    public string Login { get; set; }
    
    public string ApplicationName { get; set; }
}