using Royal.Models.Utility;
using static Royal.Service.Utilities.HttpMethods;

namespace Royal.Service.ProductService;

public sealed class ProductService : IProductService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<ProductService> logger;

    private readonly HttpClient client;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ProductService(
        IHttpClientFactory httpClientFactory,
        ILogger<ProductService> logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;

        this.client = httpClientFactory.CreateClient("DummyApiClient");
        jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    public async Task<(ProductDto, HttpStatusCode)> GetProductByIdAsync(int id)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductByIdAsync)}] Initializing HTTP Request");
;
            var response = await client.GetAsync($"products/{id}");

            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductByIdAsync)}] StatusCode for request: {response.StatusCode}");

            return await ResolveResponse<ProductDto, ProductService>(response, logger, nameof(GetProductByIdAsync));
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Product. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<(IEnumerable<ProductDto>, HttpStatusCode)> GetProductsAsync(
        int descriptionLength,
        int limit,
        int skip)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsAsync)}] Initializing HTTP Request");

            var response = await client.GetAsync($"products");

            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsAsync)}] StatusCode for request: {response.StatusCode}");

            var (productsRes, statusCode) = await ResolveResponse<ProductResponse, ProductService>(response, logger, nameof(GetProductsAsync));

            if (statusCode.Equals(HttpStatusCode.OK))
            {
                var products = limit > 0 ?
                productsRes.Products
                    .Where(p => p.Description.Length <= descriptionLength)
                    .Skip(skip)
                    .Take(limit) :
                productsRes.Products
                    .Where(p => p.Description.Length <= descriptionLength)
                    .Skip(skip);

                return (products, statusCode);
            }

            return (productsRes.Products, statusCode);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Product. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<(IEnumerable<ProductDto>, HttpStatusCode)> GetProductsByCategoryAndPriceAsync(
        string category,
        decimal minPrice,
        decimal maxPrice)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsByCategoryAndPriceAsync)}] Initializing HTTP Request");

            if (!await IsValidCategory(category))
                return (null, HttpStatusCode.NotFound);

            var response = await client.GetAsync($"products/category/{category}");

            var (productsRes, statusCode) = await ResolveResponse<ProductResponse, ProductService>(response, logger, nameof(GetProductsByCategoryAndPriceAsync));
            if (statusCode.Equals(HttpStatusCode.OK))
            {
                var products = maxPrice > 0.00m ?
                    productsRes.Products
                        .Where(p => p.Price >= minPrice)
                        .Where(p => p.Price <= maxPrice) :
                    productsRes.Products
                        .Where(p => p.Price >= minPrice);

                return (products, statusCode);
            }

            return (productsRes.Products, statusCode);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Product. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<(IEnumerable<ProductDto>, HttpStatusCode)> GetProductsByProductNameAsync(string productName)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(GetProductsByProductNameAsync)}] Initializing HTTP Request");

            var response = await client.GetAsync($"products/search?q={productName}");


            var (productsRes, statusCode) = await ResolveResponse<ProductResponse, ProductService>(response, logger, nameof(GetProductsByProductNameAsync));

            return (productsRes.Products, statusCode);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Products. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    private async Task<bool> IsValidCategory(string category)
    {
        try
        {
            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(IsValidCategory)}] Initializing HTTP Request");

            var response = await client.GetAsync($"products/category-list");

            logger.LogInformation($"[Service = {nameof(ProductService)}] [Method = {nameof(IsValidCategory)}] StatusCode for request: {response.StatusCode}");

            var (categoryRes, statusCode) = await ResolveResponse<string[], ProductService>(response, logger, nameof(IsValidCategory));

            if (statusCode.Equals(HttpStatusCode.OK)) return categoryRes.Contains(category);

            return false;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}
