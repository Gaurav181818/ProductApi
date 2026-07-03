using Microsoft.EntityFrameworkCore;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Data.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetAllAsync(PaginationParams p)
    {
        var query = context.Products.AsNoTracking();
        var total = await query.CountAsync();
        var items = await query
            .OrderBy(x => x.Id)
            .Skip((p.Page - 1) * p.PageSize)
            .Take(p.PageSize)
            .ToListAsync();
        return (items, total);
    }

    public async Task<Product?> GetByIdAsync(int id) =>
        await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Product product) => await context.Products.AddAsync(product);
    public void Update(Product product) => context.Products.Update(product);
    public void Delete(Product product) => context.Products.Remove(product);
}