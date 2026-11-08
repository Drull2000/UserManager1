using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManager.Application.Contracts.Users;
using UserManager.Domain.Entities;
using UserManager.Infrastructure.Persistence;

namespace UserManager.Application.Services;

public sealed class UserService(AppDbContext context, IPasswordHasher<User> passwordHasher) : IUserService
{
    public async Task<UserResponse?> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await context.Users.AnyAsync(user => user.Email == email, cancellationToken)) return null;

        var user = new User { Id = Guid.NewGuid(), Name = request.Name.Trim(), Email = email, CreatedDate = DateTime.UtcNow };
        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);
        return Map(user);
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        return user is null ? null : Map(user);
    }

    private static UserResponse Map(User user) => new() { Id = user.Id, Name = user.Name, Email = user.Email, CreatedDate = user.CreatedDate };
}
