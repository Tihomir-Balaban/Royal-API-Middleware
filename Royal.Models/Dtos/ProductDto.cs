using Royal.Models.Dtos.Base;
using System.ComponentModel.DataAnnotations;

namespace Royal.Models.Dtos;

public sealed class ProductDto : BaseDto
{
    public string Title  { get; set; }
    public decimal Price { get; set; }

    [MaxLength(100)]
    public string Description { get; set; }
    public string[] Images { get; set; }
    public string  Category { get; set; }
}