using UserManager.Application.Contracts.Pagination;
using UserManager.Application.Contracts.Products;

namespace UserManager.Application.Services;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken);
    Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PageResponse<ProductResponse>> GetPagedAsync(ProductQueryParams query, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
