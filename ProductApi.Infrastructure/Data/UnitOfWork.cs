using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data.Repositories;

namespace ProductApi.Infrastructure.Data;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public IProductRepository Products { get; } = new ProductRepository(context);
    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
}