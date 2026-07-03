using ProductApi.Application.DTOs;

namespace ProductApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<PagedResultDto<ProductDto>> GetAllAsync(PaginationParams pagination);

        Task<ProductDto> GetByIdAsync(int id);

        Task<ProductDto> CreateAsync(CreateProductDto dto, string username);

        Task UpdateAsync(int id, UpdateProductDto dto, string username);

        Task DeleteAsync(int id);
    }
}