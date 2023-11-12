namespace InMemoryCache.Exceptions;
public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid userId)
        : base($"User with Id {userId} does not exist") { }
}
