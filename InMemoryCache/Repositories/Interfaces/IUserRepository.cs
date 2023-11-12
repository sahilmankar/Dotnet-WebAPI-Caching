using InMemoryCache.Models;

namespace InMemoryCache.Repositories.Interfaces;

public interface IUserRepository
{
    IEnumerable<User> GetUsers();
    User GetUser(Guid userId);   
    void AddUser(User user);
    void UpdateUser(Guid userId,User user);
    void RemoveUser(Guid userId);
}
