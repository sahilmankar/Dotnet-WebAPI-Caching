using DistributedCache.Services.Interfaces;
using DistributedCache.Repositories.Interfaces;
using DistributedCache.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using DistributedCache.Extensions;

namespace DistributedCache.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IDistributedCache _cache;

    public UserService(IUserRepository repository, IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        string cacheKey = CacheKeys.AllUsersKey;
        // var users = await _cache.GetDataAsync<IEnumerable<User>>(cacheKey);
        // if (users != null)
        // {
        //     return users;
        // }

        // users = await _repository.GetUsersAsync();
        // await _cache.SetDataAsync(cacheKey, users, TimeSpan.FromSeconds(20));
        // return users;

        return await _cache.GetOrCreateAsync(
            cacheKey,
            () => _repository.GetUsersAsync(),
            TimeSpan.FromSeconds(20)
        );
    }

    public async Task<User> GetUserAsync(Guid userId)
    {
        string cacheKey = CacheKeys.GetUserKey(userId);
        // var user = await _cache.GetDataAsync<User>(cacheKey);
        // if (user != null)
        // {
        //     return user;
        // }

        // user = await _repository.GetUserAsync(userId);
        // await _cache.SetDataAsync(cacheKey, user, TimeSpan.FromSeconds(20));
        // return user;

        return await _cache.GetOrCreateAsync(cacheKey,
                                             () => _repository.GetUserAsync(userId),
                                             TimeSpan.FromSeconds(20));
    }

    public async Task AddUserAsync(User user)
    {
        await _repository.AddUserAsync(user);
        await _cache.RemoveAsync(CacheKeys.AllUsersKey);
    }

    public async Task UpdateUserAsync(Guid userId, User user)
    {
        await _repository.UpdateUserAsync(userId, user);
        await InvalidateCacheAsync(userId);
    }

    public async Task RemoveUserAsync(Guid userId)
    {
        await _repository.RemoveUserAsync(userId);
        await InvalidateCacheAsync(userId);
    }

    private async Task InvalidateCacheAsync(Guid userId)
    {
        await _cache.RemoveAsync(CacheKeys.AllUsersKey);
        await _cache.RemoveAsync(CacheKeys.GetUserKey(userId));
    }
}
