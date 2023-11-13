using DistributedCache.Models;
using DistributedCache.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DistributedCache.Controllers;

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
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _service.GetUsersAsync());
    }

    [HttpGet("{userId}", Name = "GetUser")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        return Ok(await _service.GetUserAsync(userId));
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        await _service.AddUserAsync(user);
        return CreatedAtRoute(nameof(GetUser), new { userId = user.Id }, user);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] User user)
    {
        await _service.UpdateUserAsync(userId, user);
        return Ok(user);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> RemoveUser(Guid userId)
    {
        await _service.RemoveUserAsync(userId);
        return NoContent();
    }
}
