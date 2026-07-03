using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Item> Items => Set<Item>();

    //public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>(e =>
        {
            e.ToTable("Product");
            e.Property(p => p.ProductName).HasMaxLength(255).IsRequired();
            e.Property(p => p.CreatedBy).HasMaxLength(100).IsRequired();
            e.Property(p => p.ModifiedBy).HasMaxLength(100);
        });

        builder.Entity<Item>(e =>
        {
            e.ToTable("Item");
            e.HasOne(i => i.Product)
             .WithMany(p => p.Items)
             .HasForeignKey(i => i.ProductId);
        });

        builder.Entity<Product>()
    .HasIndex(p => p.ProductName)
    .HasDatabaseName("IX_Product_ProductName");


        builder.Entity<Item>()
    .HasIndex(i => i.ProductId)
    .HasDatabaseName("IX_Item_ProductId");


        builder.Entity<User>(e =>
        {
            e.ToTable("Users");

            e.HasKey(x => x.Id);

            e.Property(x => x.Username)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.PasswordHash)
                .IsRequired();

            e.Property(x => x.Role)
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.Entity<RefreshToken>(e =>
        {
            e.ToTable("RefreshTokens");

            e.HasKey(x => x.Id);

            e.Property(x => x.Token).IsRequired();

            e.Property(x => x.Username)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.ExpiresAt)
                .IsRequired();

            e.Property(x => x.IsRevoked)
                .IsRequired();
        });
    }



    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
}