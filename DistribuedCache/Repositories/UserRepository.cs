using DistributedCache.Exceptions;
using DistributedCache.Models;
using DistributedCache.Repositories.Interfaces;

namespace DistributedCache.Repositories;

public class UserRepository : IUserRepository
{
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        Console.WriteLine("call recevied by db");

        return await Task.FromResult(DbMock.users);
    }

    public async Task<User> GetUserAsync(Guid userId)
    {
        Console.WriteLine("call recevied by db");

        var user = DbMock.users.FirstOrDefault(u => u.Id == userId);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }
        return await Task.FromResult(user);
    }

    public Task AddUserAsync(User user)
    {
        DbMock.users.Add(user);
        return Task.CompletedTask;
    }

    public Task RemoveUserAsync(Guid userId)
    {
        var user = DbMock.users.FirstOrDefault(u => u.Id == userId);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }
        DbMock.users.Remove(user);
        return Task.CompletedTask;
    }

    public Task UpdateUserAsync(Guid userId, User user)
    {
        var oldUser = DbMock.users.FirstOrDefault(u => u.Id == userId);
        if (oldUser is null)
        {
            throw new UserNotFoundException(userId);
        }
        oldUser.Email = user.Email;
        oldUser.Name = user.Name;
        return Task.CompletedTask;
    }
}
