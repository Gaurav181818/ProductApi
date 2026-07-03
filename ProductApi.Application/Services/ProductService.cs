using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Exceptions;

namespace ProductApi.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;


    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<PagedResultDto<ProductDto>> GetAllAsync(PaginationParams pagination)
    {
        var (items, total) = await unitOfWork.Products.GetAllAsync(pagination);
        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);
        return new PagedResultDto<ProductDto>(
            mapper.Map<IEnumerable<ProductDto>>(items),
            total,
            pagination.Page,
            pagination.PageSize,
            totalPages
        );
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Product), id);
        return mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto, string username)
    {
        var product = new Product
        {
            ProductName = dto.ProductName,
            CreatedBy = username,
            CreatedOn = DateTime.UtcNow
        };
        await unitOfWork.Products.AddAsync(product);
        await unitOfWork.SaveChangesAsync();
        return mapper.Map<ProductDto>(product);
    }

    public async Task UpdateAsync(int id, UpdateProductDto dto, string username)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Product), id);
        product.ProductName = dto.ProductName;
        product.ModifiedBy = username;
        product.ModifiedOn = DateTime.UtcNow;
        unitOfWork.Products.Update(product);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Product), id);
        unitOfWork.Products.Delete(product);
        await unitOfWork.SaveChangesAsync();
    }
}