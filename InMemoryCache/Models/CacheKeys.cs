namespace InMemoryCache.Models;

public static class CacheKeys
{
    public const string AllUsersKey = "allusers";
    public static string GetUserKey(Guid userId) => $"user{userId}";
}
