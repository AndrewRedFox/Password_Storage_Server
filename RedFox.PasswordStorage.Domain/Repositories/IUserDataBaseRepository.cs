using RedFox.PasswordStorage.Domain.Entities;

namespace RedFox.PasswordStorage.Domain.Repositories;

public interface IUserDataBaseRepository
{
    public Task<Guid> CreateUser(UserEntity entity);

    public Task<UserEntity> GetUserById(Guid id);

    public Task<bool> UpdateUser(UserEntity entity);
    
    public Task<List<UserEntity>> GetUserByLogin(string login);
}