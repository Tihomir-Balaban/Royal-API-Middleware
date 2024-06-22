using Royal.Models.Utility;
using System.Text;
using static Royal.Service.Utilities.HttpMethods;

namespace Royal.Service.UserService;

public sealed class UserService : IUserService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<UserService> logger;

    private readonly HttpClient client;

    public UserService(
        IHttpClientFactory httpClientFactory,
        ILogger<UserService> logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;

        this.client = httpClientFactory.CreateClient("DummyApiClient");
        client.DefaultRequestHeaders.UserAgent.ParseAdd("HttpClient");
    }

    public async Task<(IEnumerable<UserDto>, HttpStatusCode)> GetUsersAsync(
        int limit,
        int skip,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(UserService)}] [Method = {nameof(GetUsersAsync)}] Initializing HTTP Request", cancellationToken);

            var response = await client.GetAsync($"users", cancellationToken);

            logger.LogInformation($"[Service = {nameof(UserService)}] [Method = {nameof(GetUsersAsync)}] StatusCode for request: {response.StatusCode}", cancellationToken);

            var (userRes, statusCode) = await ResolveResponse<UserResponse, UserService>(response, logger, nameof(GetUsersAsync), cancellationToken);

            return (userRes.Users, statusCode);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map user(s). Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<(UserDto, HttpStatusCode)> GetByIdAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(UserService)}] [Method = {nameof(GetByIdAsync)}] Initializing HTTP Request", cancellationToken);

            var response = await client.GetAsync($"users/{userId}", cancellationToken);

            logger.LogInformation($"[Service = {nameof(UserService)}] [Method = {nameof(GetByIdAsync)}] StatusCode for request: {response.StatusCode}", cancellationToken);

            return await ResolveResponse<UserDto, UserService>(response, logger, nameof(GetByIdAsync), cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map user. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<(UserDto, HttpStatusCode)> LoginUserAsync(
        LoginRequest login,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(UserService)}] [Method = {nameof(LoginUserAsync)}] Initializing HTTP Request", cancellationToken);
            var json = JsonSerializer.Serialize(login);

            var stringContent = new StringContent(json.ToLower(), Encoding.UTF8, "application/json");

            var response = await client
                .PostAsync(
                    $"auth/login",
                    stringContent,
                    cancellationToken);

            logger.LogInformation($"[Service = {nameof(UserService)}] [Method = {nameof(LoginUserAsync)}] StatusCode for request: {response.StatusCode}", cancellationToken);

            return await ResolveResponse<UserDto, UserService>(response, logger, nameof(LoginUserAsync), cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map user. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}
