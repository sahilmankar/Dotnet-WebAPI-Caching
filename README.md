# Dotnet-WebAPI-Caching

In-Memory and Distributed caching in .NET Core 7.

- [**Inmemory Caching**](#in-memory-caching-in-net)
- [**Distributed Caching**](#distributed-caching-with-redis-in-net)
- [**Cache Invalidation**](#cache-invalidation-in-net)

### What is Caching?

Caching is the technique of storing the frequently accessed data at a temporary location for quicker access in the future. This can significantly improve the performance of an application by reducing the time required for connecting with the data source frequently and sending data across the network.

ASP.NET Core supports two types of caching :

- **In-Memory Caching** – This stores data on the application server memory.
- **Distributed Caching** – This stores data on an external service that multiple application servers can share.

In GitHub project, the `DbMock` class acts as a mock database, containing a list of users for simulating database operations. Each user is represented by the `User` class within the `DbMock` class.

# In-Memory Caching in .NET

## Overview

In-memory caching is a mechanism for temporarily storing data within the application's memory, providing fast access to frequently used information. In the context of .NET, In-Memory Caching is often used in scenarios where data needs to be cached within the same process or application instance.

## Steps for Implementation

### 1. Configure In-Memory Caching in ASP.NET Core

```csharp
// In Program.cs
    builder.Services.AddMemoryCache();
    // Other service configurations...
```

### 2. Inject IMemoryCache into Service

```csharp
    private readonly IUserRepository _repository;
    private readonly IMemoryCache _cache;

    public UserService(IUserRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }
```

### 3. Use Inside Method

```csharp
public IEnumerable<User> GetUsers()
    {
        string cacheKey = CacheKeys.AllUsersKey;
        return _cache.GetOrCreate(
            cacheKey,
            entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SlidingExpiration = TimeSpan.FromSeconds(10);
                return _repository.GetUsers();
            });
    }
```

- The GetOrCreate method is used to retrieve data from the cache or add it if it doesn't exist.

# Distributed Caching with Redis in .NET

Distributed caching is a powerful technique for sharing and managing cached data across multiple instances or servers. Redis, a popular in-memory data store, is commonly used as a distributed cache in .NET applications.

# Running Redis Using Docker

Before you begin, ensure that you have [Docker](https://www.docker.com/get-started) installed on your machine.

## Steps

### 1. Pull the Redis Image

Run the following command in your terminal to pull the official Redis image from Docker Hub:

```bash
docker pull redis
```

### 2. Run Redis Container

Start a Redis container using the following command. This will expose the default Redis port (6379) on your localhost.

```bash
docker run --name redis-container -d -p 6379:6379 redis
```

Thats it Additionly You can now connect to Redis using command-line tools.

```bash
docker exec -it redis-container redis-cli

```

This will open the Redis command-line interface.
[Here](https://www.freecodecamp.org/news/how-to-learn-redis/) you can find some basic introduction and commands of redis

### 5. Stop and Remove the Container

When you're done using Redis, you can stop and remove the container:

```bash
docker stop redis-container
docker rm redis-container
```

## Implementation Steps

### 1. Install the Redis NuGet Package

Install nuget package `Microsoft.Extensions.Caching.StackExchangeRedis`

```bash
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
```

### 2. Configure Redis in ASP.NET Core


In `appsettings.json`
```bash
  "ConnectionStrings": {
    "redis": "localhost:6379"
  }
```
Read a ConnectionString of Redis from appsettings.json
In `Program.cs` file, configure Redis as the distributed cache:

```csharp
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("redis");
});
```

### 2. Inject IDistributedCache into Service

```csharp
    private readonly IUserRepository _repository;
    private readonly IDistributedCache _cache;

    public UserService(IUserRepository repository, IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }
```

### 3. Use Inside Method

```csharp
 public async Task<User> GetUserAsync(Guid userId)
    {
        string cacheKey = CacheKeys.GetUserKey(userId);

        return await _cache.GetOrCreateAsync(cacheKey,
                                             () => _repository.GetUserAsync(userId),
                                             TimeSpan.FromSeconds(20));
    }
```

- The `GetOrCreateAsync` method is used to retrieve data from the cache or add it if it doesn't exist. You can check Defination of [GetOrCreateAsync](/DistribuedCache/Extensions/DistributedCacheExtensions.cs#L44) Extension method for Distributed caching.

# Cache Invalidation in .NET

In .NET applications, cache invalidation is crucial for maintaining data accuracy and preventing stale information. We can Invalidate Cache using the `Remove` method and both absolute and sliding expiration.

### 1. Using `Remove` Method for Cache Invalidation

To invalidate a specific item in the cache, use the `Remove` method. Here's an example:

```csharp
string cachekey=$"User{id}"
_cache.Remove(cacheKey);
```

### 2. Using Cache Expirations

- **`Absolute Expiration :`**
  Set an absolute expiration time for a cached item. The item will be removed from the cache after the specified duration

- **`Sliding Expiration :`**
  Use sliding expiration to reset the expiration time each time the item is accessed. If not accessed within the specified duration, the item will be removed

```csharp
var cacheEntryOptions = new MemoryCacheEntryOptions
{
    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
     SlidingExpiration = TimeSpan.FromMinutes(5)
};
_cache.Set(cacheKey, data, cacheEntryOptions);
```
