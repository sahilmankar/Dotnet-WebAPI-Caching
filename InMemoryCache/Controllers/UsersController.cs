using InMemoryCache.Models;
using InMemoryCache.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InMemoryCache.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(_service.GetUsers());
    }

    [HttpGet("{userId}", Name = "GetUser")]
    public IActionResult GetUser(Guid userId)
    {
        return Ok(_service.GetUser(userId));
    }

    [HttpPost]
    public IActionResult AddUser([FromBody] User user)
    {
        _service.AddUser(user);
        return CreatedAtRoute(nameof(GetUser), new { userId = user.Id }, user);
    }

    [HttpPut("{userId}")]
    public IActionResult UpdateUser(Guid userId, [FromBody] User user)
    {
        _service.UpdateUser(userId, user);
        return Ok(user);
    }

    [HttpDelete("{userId}")]
    public IActionResult RemoveUser(Guid userId)
    {
        _service.RemoveUser(userId);
        return NoContent();
    }
    
}
