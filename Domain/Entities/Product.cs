namespace UserManager.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsDeleted { get; set; }
}
