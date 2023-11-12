using InMemoryCache.Services.Interfaces;
using InMemoryCache.Repositories.Interfaces;
using InMemoryCache.Models;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCache.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMemoryCache _cache;

    public UserService(IUserRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public IEnumerable<User> GetUsers()
    {
        string cacheKey = CacheKeys.AllUsersKey;
        return _cache.GetOrCreate(
            cacheKey,
            entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SlidingExpiration = TimeSpan.FromSeconds(5);
                return _repository.GetUsers();
            }
        )!;
    }

    public User GetUser(Guid userId)
    {
        string cacheKey = CacheKeys.GetUserKey(userId);
        return _cache.GetOrCreate(
            cacheKey,
            entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SlidingExpiration = TimeSpan.FromSeconds(5);
                return _repository.GetUser(userId);
            }
        )!;
    }

    public void AddUser(User user)
    {
        _repository.AddUser(user);
        _cache.Remove(CacheKeys.AllUsersKey);
    }

    public void UpdateUser(Guid userId, User user)
    {
        _repository.UpdateUser(userId, user);
        InvalidateCache(userId);
    }

    public void RemoveUser(Guid userId)
    {
        _repository.RemoveUser(userId);
        InvalidateCache(userId);
    }

    private void InvalidateCache(Guid userId)
    {
        _cache.Remove(CacheKeys.AllUsersKey);
        _cache.Remove(CacheKeys.GetUserKey(userId));
    }
}
