using System.ComponentModel.DataAnnotations;

namespace DistributedCache.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MinLength(3)]
    public required string Name { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
