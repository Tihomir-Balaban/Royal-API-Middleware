namespace Royal.Service.Tests.ProductServiceTest;

public class BaseProductServiceTest
{
    protected readonly Mock<IHttpClientFactory> HttpClientFactoryMock = new();
    protected readonly Mock<ILogger<PS.ProductService>> LoggerMock = new();

    protected PS.ProductService ProductService;

    private protected void InitService()
    {
        ProductService = new(
            HttpClientFactoryMock.Object,
            LoggerMock.Object);
    }
}