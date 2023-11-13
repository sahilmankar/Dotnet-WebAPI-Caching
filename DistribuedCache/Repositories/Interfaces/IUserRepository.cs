using DistributedCache.Models;

namespace DistributedCache.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserAsync(Guid userId);   
    Task AddUserAsync(User user);
    Task UpdateUserAsync(Guid userId,User user);
    Task RemoveUserAsync(Guid userId);
}
