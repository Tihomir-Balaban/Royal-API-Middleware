using Newtonsoft.Json;
using Royal.Models.Utility;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Royal.Service.Tests.ProductServiceTest;

public sealed class ProductServiceTest : BaseProductServiceTest
{
    [Fact]
    public async Task GetProductByIdAsync_ReturnsProductAndStatusCodeOK()
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
        var (result, statusCode) = await ProductService.GetProductByIdAsync(1, default);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Essence Mascara Lash Princess", result.Title);
        Assert.Equal("The Essence Mascara Lash Princess is a popular mascara known for its volumizing and lengthening effects. Achieve dramatic lashes with this long-lasting and cruelty-free formula.", result.Description);
        Assert.Equal(9.99m, result.Price);
        Assert.Equal("beauty", result.Category);
        Assert.Equal(new[] { "https://cdn.dummyjson.com/products/images/beauty/Essence%20Mascara%20Lash%20Princess/1.png" }, result.Images);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsStatusCodeNotFound()
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
        var (result, statusCode) = await ProductService.GetProductByIdAsync(195, default);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, statusCode);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetProductsAsync_ReturnsFilteredProductsAndStatusCodeOK()
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
        var (result, statusCode) = await ProductService.GetProductsAsync(100, 2, 1, default);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, p => p.Id == 17);
        Assert.Contains(result, p => p.Id == 18);
    }

    [Fact]
    public async Task GetProductsAsync_ReturnsAllProductsWhenLimitIsZero()
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
        var (result, statusCode) = await ProductService.GetProductsAsync(int.MaxValue, 0, 0, default);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(result);
        Assert.Equal(30, result.Count());
    }

    [Fact]
    public async Task GetProductsByCategoryAndPriceAsync_ReturnsFilteredProductsAndStatusCodeOK()
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
        var (result, statusCode) = await ProductService.GetProductsByCategoryAndPriceAsync("beauty", 10.0m, 20.0m, default);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(result);
        Assert.Contains(result, p => p.Id == 2);
        Assert.Contains(result, p => p.Id == 3);
        Assert.Contains(result, p => p.Id == 4);
    }

    [Fact]
    public async Task GetProductsByCategoryAndPriceAsync_ReturnsProductsWhenMaxPriceIsZero()
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
        var (result, statusCode) = await ProductService.GetProductsByCategoryAndPriceAsync("fragrances", 10.0m, 0.0m, default);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(result);
        Assert.Contains(result, p => p.Id == 6);
        Assert.Contains(result, p => p.Id == 7);
        Assert.Contains(result, p => p.Id == 8);
        Assert.Contains(result, p => p.Id == 9);
        Assert.Contains(result, p => p.Id == 10);
    }

    [Fact]
    public async Task GetProductsByProductNameAsync_ReturnsProductsAndStatusCodeOK()
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
        var (result, statusCode) = await ProductService.GetProductsByProductNameAsync("Product", default);

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Contains(result, p => p.Title == "Asus Zenbook Pro Dual Screen Laptop");
        Assert.Contains(result, p => p.Title == "TV Studio Camera Pedestal");
        Assert.Contains(result, p => p.Title == "Samsung Galaxy Tab S8 Plus Grey");
    }
}