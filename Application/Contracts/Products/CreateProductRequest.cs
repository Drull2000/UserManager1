namespace UserManager.Application.Contracts.Products;

public sealed class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
