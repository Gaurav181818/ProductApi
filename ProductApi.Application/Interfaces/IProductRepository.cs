using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<(IEnumerable<Product> Items, int TotalCount)> GetAllAsync(PaginationParams pagination);
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
    }

    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        Task<int> SaveChangesAsync();
    }
}
