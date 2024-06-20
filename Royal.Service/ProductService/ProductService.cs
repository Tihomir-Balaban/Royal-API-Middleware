using Microsoft.Extensions.Logging;
using Royal.Models.Utility;
using System.Collections.Generic;
using System.Text.Json;

namespace Royal.Service.ProductService;

public sealed class ProductService : IProductService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<ProductService> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;
    public ProductService(
        IHttpClientFactory httpClientFactory,
        ILogger<ProductService> logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;
        jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    public async Task<(ProductDto, bool)> GetProductByIdAsync(int id)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductByIdAsync)}] Initializing HTTP Request");

            var client = httpClientFactory.CreateClient("ProductApiClient");
            var response = await client.GetAsync($"products/{id}");

            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductByIdAsync)}] StatusCode for request: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductByIdAsync)}] Initializing deserialization of content");

                var content = await response.Content.ReadAsStringAsync();
                var product = JsonSerializer.Deserialize<ProductDto>(json: content, jsonSerializerOptions);

                logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductByIdAsync)}] Content deserialized");

                return (product, response.IsSuccessStatusCode);
            }

            return (null, response.IsSuccessStatusCode);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Product. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<(IEnumerable<ProductDto>, bool)> GetProductsAsync(
        int descriptionLength,
        int limit,
        int skip)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsAsync)}] Initializing HTTP Request");

            var client = httpClientFactory.CreateClient("ProductApiClient");
            var response = await client.GetAsync($"products");

            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsAsync)}] StatusCode for request: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsAsync)}] Initializing deserialization of content");

                var content = await response.Content.ReadAsStringAsync();
                var productsResponse = JsonSerializer.Deserialize<ProductResponse>(content, jsonSerializerOptions);

                var products = limit > 0 ?
                    productsResponse.Products
                        .Where(p => p.Description.Length <= descriptionLength)
                        .Skip(skip)
                        .Take(limit) :
                    productsResponse.Products
                        .Where(p => p.Description.Length <= descriptionLength)
                        .Skip(skip);

                logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsAsync)}] Content deserialized, found {products.Count()} Product(s)");

                return (products, response.IsSuccessStatusCode);
            }

            return (null, response.IsSuccessStatusCode);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Product. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<(IEnumerable<ProductDto>, bool)> GetProductsByCategoryAndPriceAsync(
        string category,
        decimal minPrice,
        decimal maxPrice)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsByCategoryAndPriceAsync)}] Initializing HTTP Request");

            if (!await IsValidCategory(category))
                return (null, false);

            var client = httpClientFactory.CreateClient("ProductApiClient");
            var response = await client.GetAsync($"products/category/{category}");

            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsByCategoryAndPriceAsync)}] StatusCode for request: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsByCategoryAndPriceAsync)}] Initializing deserialization of content");

                var content = await response.Content.ReadAsStringAsync();
                var productsResponse = JsonSerializer.Deserialize<ProductResponse>(content, jsonSerializerOptions);

                var products = maxPrice > 0.00m ?
                    productsResponse.Products
                        .Where(p => p.Category == category)
                        .Where(p => p.Price >= minPrice)
                        .Where(p => p.Price <= maxPrice) :
                    productsResponse.Products
                        .Where(p => p.Category == category)
                        .Where(p => p.Price >= minPrice);

                logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsByCategoryAndPriceAsync)}] Content deserialized, found {products.Count()} Product(s)");

                return (products, response.IsSuccessStatusCode);
            }

            return (null, response.IsSuccessStatusCode);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Product. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<(IEnumerable<ProductDto>, bool)> GetProductsByProductNameAsync(string productName)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsByProductNameAsync)}] Initializing HTTP Request");

            var client = httpClientFactory.CreateClient("ProductApiClient");
            var response = await client.GetAsync($"products/search?q={productName}");

            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsByProductNameAsync)}] StatusCode for request: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsByProductNameAsync)}] Initializing deserialization of content");

                var content = await response.Content.ReadAsStringAsync();
                var productsResponse = JsonSerializer.Deserialize<ProductResponse>(content, jsonSerializerOptions);

                logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsByProductNameAsync)}] Content deserialized, found {products.Count()} Product(s)");

                return (productsResponse.Products, response.IsSuccessStatusCode);
            }

            return (null, response.IsSuccessStatusCode);
        }
        catch (Exception e)
        {
        }
    }

    private async Task<bool> IsValidCategory(string category)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(IsValidCategory)}] Initializing HTTP Request");

            var client = httpClientFactory.CreateClient("ProductApiClient");
            var response = await client.GetAsync($"products/category-list");

            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(IsValidCategory)}] StatusCode for request: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {

                logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(IsValidCategory)}] Initializing deserialization of content and checking if it contains requested category: {category}");

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer
                    .Deserialize<string[]>(content, jsonSerializerOptions)
                    .Contains(category);
            }

            return false;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}