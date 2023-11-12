using InMemoryCache.Models;

namespace InMemoryCache.Repositories;

public class DbMock 
{
    public static List<User> users = new List<User>()
    {
        new User
        {
            Id = new Guid("f26f61c0-f836-46af-992e-18df5d2d4a31"),
            Name = "Sahil Mankar",
            Email = "sahil123@gmail.com"
        },
        new User
        {
            Id = new Guid("9fc2bf90-3318-4126-9e91-cabcbe7aa2ea"),
            Name = "Abhay Navale",
            Email = "navle12@gmail.com"
        },
        new User
        {
            Id = new Guid("1c745bdd-8b61-404a-b58c-2475d14cecd8"),
            Name = "Shubham Teli",
            Email = "shumila1203@gmail.com"
        },
        new User
        {
            Id = new Guid("056e3d83-691e-4782-bfca-6ff18de0c85c"),
            Name = "Vedant Yadav",
            Email = "vedantyadav@gmail.com"
        },
        new User
        {
            Id = new Guid("ce5ab341-8ab1-4379-802c-426f76599d20"),
            Name = "Roshan Gadhve",
            Email = "roshan@gmail.com"
        }
    };
}