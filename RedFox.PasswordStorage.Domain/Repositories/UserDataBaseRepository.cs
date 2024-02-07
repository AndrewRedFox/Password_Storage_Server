using MongoDB.Driver;
using RedFox.PasswordStorage.Domain.Entities;

namespace RedFox.PasswordStorage.Domain.Repositories;

public sealed class UserDataBaseRepository : IUserDataBaseRepository
{
    private readonly string _connectionString = "mongodb://admin:touareg34Hjk@localhost:27017/simple_db?ssl=false&authSource=admin";
    private readonly string _databaseName = "simple_db";
    private readonly string _userCollection = "users";
    
    private IMongoCollection<T> _connectToMongo<T>(in string collection)
    {
        var client = new MongoClient(_connectionString);
        var db = client.GetDatabase(_databaseName);
        return db.GetCollection<T>(collection);
    }
    public async Task<Guid> CreateUser(UserEntity entity)
    {
        var userCollection = _connectToMongo<UserEntity>(_userCollection);
        await userCollection.InsertOneAsync(entity);
        return entity.Id;
    }

    public async Task<UserEntity> GetUserById(Guid Id)
    {
        var userCollection = _connectToMongo<UserEntity>(_userCollection);
        var result = await userCollection.FindAsync(c => c.Id == Id);
        return result.ToList()[0];
    }
    public async Task<List<UserEntity>> GetUserByLogin(string login)
    {
        var userCollection = _connectToMongo<UserEntity>(_userCollection);
        var result = await userCollection.FindAsync(c => c.Login == login);
        return result.ToList();
    }

    public async Task<bool> UpdateUser(UserEntity entity)
    {
        var userCollection = _connectToMongo<UserEntity>(_userCollection);
        var filter = Builders<UserEntity>.Filter.Eq("Id", entity.Id);
        await userCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = false });
        return true;
    }
}