using InMemoryCache.Exceptions;
using InMemoryCache.Models;
using InMemoryCache.Repositories.Interfaces;

namespace InMemoryCache.Repositories;

public class UserRepository : IUserRepository
{
  
    public IEnumerable<User> GetUsers()
    {
        return DbMock.users;
    }

    public User GetUser(Guid userId)
    {
        var user = DbMock.users.FirstOrDefault(u => u.Id == userId);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }
        return user;
    }

    public void AddUser(User user)
    {
        DbMock.users.Add(user);
    }

    public void RemoveUser(Guid userId)
    {
        var user = DbMock.users.FirstOrDefault(u => u.Id == userId);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }
        DbMock.users.Remove(user);
    }

    public void UpdateUser(Guid userId, User user)
    {
        var oldUser = DbMock.users.FirstOrDefault(u => u.Id == userId);
        if (oldUser is null)
        {
            throw new UserNotFoundException(userId);
        }
        oldUser.Email = user.Email;
        oldUser.Name = user.Name;
    }
}
