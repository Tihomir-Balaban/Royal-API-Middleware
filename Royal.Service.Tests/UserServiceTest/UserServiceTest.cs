using Newtonsoft.Json;
using Royal.Models.Utility;
using System.Net;
using System.Text;
using Xunit;

namespace Royal.Service.Tests.UserServiceTest;

public sealed class UserServiceTest : BaseUserServiceTest
{
    [Fact]
    public async Task GetUsersAsync_ReturnsUsersAndStatusCodeOK()
    {
        // Arrange
        HttpClientFactoryMock
            .Setup(h => h.CreateClient("DummyApiClient"))
            .Returns(new HttpClient()
            {
                BaseAddress = new Uri("http://dummyjson.com/")
            });

        InitService();

        // Act
        var (result, statusCode) = await UserService.GetUsersAsync(0, 0, default);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(result);
        Assert.Equal(30, result.Count());
        Assert.Contains(result, u => u.UserName == "emilys");
        Assert.Contains(result, u => u.UserName == "michaelw");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsUserAndStatusCodeOK()
    {
        // Arrange
        HttpClientFactoryMock
            .Setup(h => h.CreateClient("DummyApiClient"))
            .Returns(new HttpClient()
            {
                BaseAddress = new Uri("http://dummyjson.com/")
            });

        InitService();

        // Act
        var (result, statusCode) = await UserService.GetByIdAsync(1, default);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("emilys", result.UserName);
        Assert.Equal("emily.johnson@x.dummyjson.com", result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsStatusCodeNotFound()
    {
        // Arrange
        HttpClientFactoryMock
            .Setup(h => h.CreateClient("DummyApiClient"))
            .Returns(new HttpClient()
            {
                BaseAddress = new Uri("http://dummyjson.com/")
            });

        InitService();

        // Act
        var (result, statusCode) = await UserService.GetByIdAsync(209, default);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, statusCode);
        Assert.NotNull(result);
    }
}