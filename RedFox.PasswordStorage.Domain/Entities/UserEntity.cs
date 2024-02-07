namespace RedFox.PasswordStorage.Domain.Entities;

public sealed class UserEntity : Entity
{
    public required string Login { get; set; }

    public required string Password { get; set; }

    public string? AccessToken { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public List<UserInfoEntity?> UserInfo { get; set; }
}