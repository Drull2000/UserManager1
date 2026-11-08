using Microsoft.EntityFrameworkCore;
using UserManager.Application.Contracts.Pagination;
using UserManager.Application.Contracts.Products;
using UserManager.Domain.Entities;
using UserManager.Infrastructure.Persistence;

namespace UserManager.Application.Services;

public sealed class ProductService(AppDbContext context) : IProductService
{
    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Price = request.Price,
            Description = request.Description.Trim(),
            Quantity = request.Quantity,
            CreatedDate = DateTime.UtcNow
        };

        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);
        return Map(product);
    }

    public async Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await context.Products.AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id && !product.IsDeleted, cancellationToken);
        return product is null ? null : Map(product);
    }

    public async Task<PageResponse<ProductResponse>> GetPagedAsync(ProductQueryParams query, CancellationToken cancellationToken)
    {
        var products = context.Products.AsNoTracking().Where(product => !product.IsDeleted);
        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            var search = query.Name.Trim();
            products = products.Where(product => product.Name.Contains(search) || product.Description.Contains(search));
        }

        if (query.MinPrice.HasValue) products = products.Where(product => product.Price >= query.MinPrice.Value);
        if (query.MaxPrice.HasValue) products = products.Where(product => product.Price <= query.MaxPrice.Value);

        products = query.SortBy?.ToLowerInvariant() switch
        {
            "name" => query.IsSortDescending ? products.OrderByDescending(product => product.Name) : products.OrderBy(product => product.Name),
            "price" => query.IsSortDescending ? products.OrderByDescending(product => product.Price) : products.OrderBy(product => product.Price),
            _ => query.IsSortDescending ? products.OrderByDescending(product => product.CreatedDate) : products.OrderBy(product => product.CreatedDate)
        };

        var totalCount = await products.CountAsync(cancellationToken);
        var items = await products.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToListAsync(cancellationToken);
        return new PageResponse<ProductResponse>(items.Select(Map).ToList(), totalCount, query.PageNumber, query.PageSize);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await context.Products.FirstOrDefaultAsync(product => product.Id == id && !product.IsDeleted, cancellationToken);
        if (product is null) return false;
        product.IsDeleted = true;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static ProductResponse Map(Product product) => new()
    {
        Id = product.Id, Name = product.Name, Price = product.Price,
        Description = product.Description, Quantity = product.Quantity, CreatedDate = product.CreatedDate
    };
}
