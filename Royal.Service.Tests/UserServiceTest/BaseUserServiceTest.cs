namespace Royal.Service.Tests.UserServiceTest;

public class BaseUserServiceTest
{
    protected readonly Mock<IHttpClientFactory> HttpClientFactoryMock = new();
    protected readonly Mock<ILogger<US.UserService>> LoggerMock = new();

    protected US.UserService UserService;

    private protected void InitService()
    {
        UserService = new(
            HttpClientFactoryMock.Object,
            LoggerMock.Object);
    }
}