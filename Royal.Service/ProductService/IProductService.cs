namespace Royal.Service.ProductService;

public interface IProductService
{
    Task<(IEnumerable<ProductDto>, bool)> GetProductsAsync(int descriptionLength, int limit, int skip);
    Task<(ProductDto, bool)> GetProductByIdAsync(int id);
    Task<(IEnumerable<ProductDto>, bool)> GetProductsByCategoryAndPriceAsync(string category, decimal minPrice, decimal maxPrice);
    Task<(IEnumerable<ProductDto>, bool)> GetProductsByProductNameAsync(string productName);
}
