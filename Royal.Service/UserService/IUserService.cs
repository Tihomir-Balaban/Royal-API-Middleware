using Royal.Models.Utility;

namespace Royal.Service.UserService;

public interface IUserService
{
    Task<(IEnumerable<UserDto>, HttpStatusCode)> GetUsersAsync(int limit, int skip, CancellationToken cancellationToken);
    Task<(UserDto, HttpStatusCode)> GetByIdAsync(int userId, CancellationToken cancellationToken);
    Task<(UserDto, HttpStatusCode)> LoginUserAsync(LoginRequest login, CancellationToken cancellationToken); 
}
