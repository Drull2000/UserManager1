using UserManager.Application.Contracts.Users;

namespace UserManager.Application.Services;

public interface IUserService
{
    Task<UserResponse?> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken);
    Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
