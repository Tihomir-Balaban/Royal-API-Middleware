﻿using Royal.Service.ProductService;

namespace Royal.API.Middleware.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ProductController : Controller
{
    private readonly ILogger<ProductController> logger;
    private readonly IProductService productService;

    public ProductController(
        IProductService productService,
        ILogger<ProductController> logger)
    {
        this.productService = productService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <example>
    ///     Example of ProductDtos in json array form
    ///     [
    ///         {
    ///             "id": int,
    ///             "title": string,
    ///             "price" decimal,
    ///             "description": string,
    ///             "images": string,
    ///             "category": string
    ///         },
    ///         {
    ///             "id": int,
    ///             "title": string,
    ///             "price" decimal,
    ///             "description": string,
    ///             "images": string,
    ///             "category": string
    ///         },
    ///         {
    ///             ...
    ///         }
    ///     ]
    /// </example>
    /// <response code="200">Returns the list of products</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">Returns message if there are no products in the Api/Database</response>
    // GET: /Product
    // The follow request query can be mixed and matched all incomming parameters have default preset values
    // GET: /Product?limit={limit}&skip={skip}&descriptionLength={descriptionLength}
    //[Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
        [FromQuery] int descriptionLength = 100,
        [FromQuery] int limit = 0,
        [FromQuery] int skip = 0)
    {
        logger.LogInformation("Triggered Endpoint GET: /product");

        try
        {
            logger.LogInformation("Triggering Product Service: GetProductsAsync");

            var (products, isSuccessStatusCode) = await productService.GetProductsAsync(
            descriptionLength,
            limit,
            skip);

            if (isSuccessStatusCode)
                return Ok(products);

            logger.LogWarning("No Products not found.");

            return NotFound(products);

        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves product by id.
    /// </summary>
    /// <example>
    ///     Example of ProductDto in json array form
    ///     {
    ///         "id": int,
    ///         "title": string,
    ///         "price" decimal,
    ///         "description": string,
    ///         "images": string,
    ///         "category": string
    ///     }
    /// </example>
    /// <response code="200">Returns the list of product</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">Returns message if there is no product in the Api/Database with that id</response>
    // GET: /Product/{id}
    //[Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductById(int id)
    {
        logger.LogInformation("Triggered Endpoint GET: /product/{id}");

        try
        {
            logger.LogInformation("Triggering Product Service: GetProductByIdAsync");

            var (products, isSuccessStatusCode) = await productService.GetProductByIdAsync(id);

            if (isSuccessStatusCode)
                return Ok(products);

            logger.LogWarning("No Products not found.");

            return NotFound((products));
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves products by Category and Price.
    /// </summary>
    /// <example>
    ///     Example of ProductDtos in json array form
    ///     [
    ///         {
    ///             "id": int,
    ///             "title": string,
    ///             "price" decimal,
    ///             "description": string,
    ///             "images": string,
    ///             "category": string
    ///         },
    ///         {
    ///             "id": int,
    ///             "title": string,
    ///             "price" decimal,
    ///             "description": string,
    ///             "images": string,
    ///             "category": string
    ///         },
    ///         {
    ///             ...
    ///         }
    ///     ]
    /// </example>
    /// <response code="200">Returns the list of products</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">Returns message if there is no product in the Api/Database with that category and/or price</response>
    // GET: /Product/Category/{category}
    // GET: /Product/Category/{category}?minPrice=1.99&maxPrice=2.99
    //[Authorize]
    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategoryAndPrice(
        string category,
        [FromQuery] decimal minPrice = 0.00m,
        [FromQuery] decimal maxPrice = 0.00m)
    {
        logger.LogInformation("Triggered Endpoint GET: /Product/Category/{category}");

        try
        {
            if (!maxPrice.Equals(0.00m) &&
                minPrice > maxPrice)
                return BadRequest("Invalid Query values minimum price value larger than maximum price value!");

            logger.LogInformation("Triggering Product Service: GetProductsByCategoryAndPriceAsync");

            var (products, isSuccessStatusCode) = await productService.GetProductsByCategoryAndPriceAsync(category, minPrice, maxPrice);

            if (isSuccessStatusCode)
                return Ok(products);

            logger.LogWarning("No Products not found.");

            return NotFound(products);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves products by Name.
    /// </summary>
    /// <example>
    ///     Example of ProductDtos in json array form
    ///     [
    ///         {
    ///             "id": int,
    ///             "title": string,
    ///             "price" decimal,
    ///             "description": string,
    ///             "images": string,
    ///             "category": string
    ///         },
    ///         {
    ///             "id": int,
    ///             "title": string,
    ///             "price" decimal,
    ///             "description": string,
    ///             "images": string,
    ///             "category": string
    ///         },
    ///         {
    ///             ...
    ///         }
    ///     ]
    /// </example>
    /// <response code="200">Returns the list of products</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">Returns message if there is no product in the Api/Database with that name</response>
    // GET: /Product/Category/{category}
    // GET: /Product/Category/{category}?minPrice=1.99&maxPrice=2.99
    //[Authorize]
    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByProductName([FromQuery] string productName = "")
    {
        logger.LogInformation("Triggered Endpoint GET: /Product/Category/{category}");

        try
        {
            logger.LogInformation("Triggering Product Service: GetProductsByProductNameAsync");

            var (products, isSuccessStatusCode) = await productService.GetProductsByProductNameAsync(productName);

            if (isSuccessStatusCode)
                return Ok(products);

            logger.LogWarning("No Products not found.");

            return NotFound(products);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }
}