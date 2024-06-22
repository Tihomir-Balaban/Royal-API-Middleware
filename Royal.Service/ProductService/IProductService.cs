namespace Royal.Service.ProductService;

public interface IProductService
{
    Task<(IEnumerable<ProductDto>, HttpStatusCode)> GetProductsAsync(int descriptionLength, int limit, int skip);
    Task<(ProductDto, HttpStatusCode)> GetProductByIdAsync(int id);
    Task<(IEnumerable<ProductDto>, HttpStatusCode)> GetProductsByCategoryAndPriceAsync(string category, decimal minPrice, decimal maxPrice);
    Task<(IEnumerable<ProductDto>, HttpStatusCode)> GetProductsByProductNameAsync(string productName);
}
