﻿using Royal.API.Middleware.Controllers.Base;
using Royal.Service.ProductService;
using System.Threading;

namespace Royal.API.Middleware.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ProductController : RoyalController
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
    // The following request query can be mixed and matched all incomming parameters have default preset values
    // GET: /Product?limit={limit}&skip={skip}&descriptionLength={descriptionLength}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
        CancellationToken cancellationToken,
        [FromQuery] int descriptionLength = 100,
        [FromQuery] int limit = 0,
        [FromQuery] int skip = 0)
    {
        logger.LogInformation("Triggered Endpoint GET: /product", cancellationToken);

        try
        {
            logger.LogInformation("Triggering Product Service: GetProductsAsync", cancellationToken);

            var (products, statusCode) = await productService.GetProductsAsync(
                descriptionLength,
                limit,
                skip,
                cancellationToken);

            return HttpStatusCodeResolve(products, statusCode);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}", cancellationToken);

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
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Triggered Endpoint GET: /product/{id}", cancellationToken);

        try
        {
            logger.LogInformation("Triggering Product Service: GetProductByIdAsync", cancellationToken);

            var (products, statusCode) = await productService.GetProductByIdAsync(id, cancellationToken);

            return HttpStatusCodeResolve(products, statusCode);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}", cancellationToken);

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
    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategoryAndPrice(
        string category, 
        CancellationToken cancellationToken,
        [FromQuery] decimal minPrice = 0.00m,
        [FromQuery] decimal maxPrice = 0.00m)
    {
        logger.LogInformation("Triggered Endpoint GET: /Product/Category/{category}", cancellationToken);

        try
        {
            if (!maxPrice.Equals(0.00m) &&
                minPrice > maxPrice)
                return BadRequest("Invalid Query values minimum price value larger than maximum price value!");

            logger.LogInformation("Triggering Product Service: GetProductsByCategoryAndPriceAsync", cancellationToken);

            var (products, statusCode) = await productService.GetProductsByCategoryAndPriceAsync(category, minPrice, maxPrice, cancellationToken);

            return HttpStatusCodeResolve(products, statusCode);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}", cancellationToken);

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
    // GET: /Product/Name
    // GET: /Product/Name?productName=someproduct
    [HttpGet("name")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByProductName(
        CancellationToken cancellationToken,
        [FromQuery] string productName = "")
    {
        logger.LogInformation("Triggered Endpoint GET: /Product/Category/{category}", cancellationToken);

        try
        {
            logger.LogInformation("Triggering Product Service: GetProductsByProductNameAsync", cancellationToken);

            var (products, statusCode) = await productService.GetProductsByProductNameAsync(productName, cancellationToken);

            return HttpStatusCodeResolve(products, statusCode);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}", cancellationToken);

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }
}
