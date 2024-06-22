using Royal.API.Middleware.Controllers.Base;
using Royal.Common.Enum;
using Royal.Models.Utility;
using Royal.Service.Security;
using Royal.Service.UserService;
using System.Net;

namespace Royal.API.Middleware.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class UserController : RoyalController
{
    private readonly IUserService userService;
    private readonly ISecurityService securityService;
    private readonly ILogger<UserController> logger;

    public UserController(
        IUserService userService,
        ISecurityService securityService,
        ILogger<UserController> logger)
    {
        this.userService = userService;
        this.securityService = securityService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <example>
    ///     Example of UserDtos in json array form
    ///     [
    ///         {
    ///             "id": int,
    ///             "firstname": string,
    ///             "lastname": string,
    ///             "email": string,
    ///             "password": string,
    ///             "role": int
    ///         },
    ///         {
    ///             "id": int,
    ///             "firstname": string,
    ///             "lastname": string,
    ///             "email": string,
    ///             "password": string,
    ///             "role": int
    ///         },
    ///         {
    ///             ...
    ///         }
    ///     ]
    /// </example>
    /// <response code="200">Returns the list of users</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">Returns message if that there are no users in the Database</response>
    // GET: /User
    // The following request query can be mixed and matched all incomming parameters have default preset values
    // GET: /User?limit={limit}&skip={skip}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
        CancellationToken cancellationToken,
        [FromQuery] int limit = 0,
        [FromQuery] int skip = 0)
    {
        logger.LogInformation("Triggered Endpoint GET: /user");

        try
        {
            logger.LogInformation("Triggering User Service: GetUsersAsync");

            var (users, statusCode) = await userService.GetUsersAsync(limit, skip, cancellationToken);

            return HttpStatusCodeResolve(users, statusCode);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <example>
    ///     Example of UserDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <response code="200">Returns the user corresponding to the specified ID</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">If no user is found with the specified ID</response>
    // GET: /User/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Triggered Endpoint GET: user/{id}");

        try
        {
            logger.LogInformation("Triggering User Service: GetByIdAsync");

            var (user, statusCode) = await userService.GetByIdAsync(id, cancellationToken);

            return HttpStatusCodeResolve(user, statusCode);

        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Will log user in by username and password.
    /// </summary>
    /// <example>
    ///     Example of LoginRequest in json form:
    ///     {
    ///         "username": string,
    ///         "email": string,
    ///         "password": string,
    ///     }
    /// </example>
    /// <param name="login"></param>
    // POST: User/login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login, CancellationToken cancellationToken)
    {
        var (user, statusCode) = await userService.LoginUserAsync(login, cancellationToken);

        if (user == null)
            return Unauthorized();

        
        var (userWithRole, statusCodeTwo) = await userService.GetByIdAsync(user.Id, cancellationToken);

        user.Role = userWithRole.Role;

        var tokenString = securityService.JwtSecurityHandler(user);

        return Ok(new { Token = tokenString });
    }
}
