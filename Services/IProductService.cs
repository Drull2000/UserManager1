using UserManager.DTO.Pagination;
using UserManager.DTO.Product;

namespace UserManager.Services
{
    public interface IProductService
    {
        Task<PageResponse<ProductDTO>> GetPagedAsync(ProductQueryParams queryParams);
        Task<ProductDTO?> GetProductByIdAsync(Guid id);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO product);
        Task<bool> DeleteAsync(Guid id);
    }
}

