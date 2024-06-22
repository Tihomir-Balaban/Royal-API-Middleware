namespace Royal.Service.ProductService;

public interface IProductService
{
    Task<(IEnumerable<ProductDto>, HttpStatusCode)> GetProductsAsync(int descriptionLength, int limit, int skip, CancellationToken cancellationToken);
    Task<(ProductDto, HttpStatusCode)> GetProductByIdAsync(int id, CancellationToken cancellationToken);
    Task<(IEnumerable<ProductDto>, HttpStatusCode)> GetProductsByCategoryAndPriceAsync(string category, decimal minPrice, decimal maxPrice, CancellationToken cancellationToken);
    Task<(IEnumerable<ProductDto>, HttpStatusCode)> GetProductsByProductNameAsync(string productName, CancellationToken cancellationToken);
}
