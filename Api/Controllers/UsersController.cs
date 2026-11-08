using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserManager.Application.Contracts.Users;
using UserManager.Application.Services;

namespace UserManager.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(IUserService userService, IValidator<RegisterUserRequest> validator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid) return BadRequest(validation.ToDictionary());
        var user = await userService.RegisterAsync(request, cancellationToken);
        return user is null ? Conflict("A user with this email already exists.") : CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await userService.GetByIdAsync(id, cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }
}
