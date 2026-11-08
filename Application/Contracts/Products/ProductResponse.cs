namespace UserManager.Application.Contracts.Products;

public sealed class ProductResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public DateTime CreatedDate { get; init; }
}
