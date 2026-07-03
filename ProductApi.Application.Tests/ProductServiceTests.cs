using AutoMapper;
using FluentAssertions;
using Moq;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Application.Services;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Exceptions;
using Xunit;

namespace ProductApi.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock
                .Setup(x => x.Products)
                .Returns(_repositoryMock.Object);

            _service = new ProductService(
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        #region GetById

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            var dto = new ProductDto(
                product.Id,
                product.ProductName,
                product.CreatedBy,
                product.CreatedOn);

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            _mapperMock
                .Setup(x => x.Map<ProductDto>(product))
                .Returns(dto);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().Be(dto);

            _repositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Product?)null);

            // Act
            Func<Task> act = async () =>
                await _service.GetByIdAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();
        }

        #endregion

        #region Create

        [Fact]
        public async Task CreateAsync_ShouldCreateProduct()
        {
            // Arrange
            var dto = new CreateProductDto("Laptop");

            var createdProduct = new Product
            {
                Id = 1,
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            var expectedDto = new ProductDto(
                createdProduct.Id,
                createdProduct.ProductName,
                createdProduct.CreatedBy,
                createdProduct.CreatedOn);

            _mapperMock
                .Setup(x => x.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(expectedDto);

            // Act
            var result = await _service.CreateAsync(dto, "Admin");

            // Assert
            _repositoryMock.Verify(x =>
                x.AddAsync(It.Is<Product>(p =>
                    p.ProductName == dto.ProductName &&
                    p.CreatedBy == "Admin")),
                Times.Once);

            _unitOfWorkMock.Verify(x =>
                x.SaveChangesAsync(),
                Times.Once);

            result.Should().Be(expectedDto);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ProductName = "Old Name"
            };

            var dto = new UpdateProductDto("New Name");

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act
            await _service.UpdateAsync(1, dto, "Admin");

            // Assert
            product.ProductName.Should().Be("New Name");
            product.ModifiedBy.Should().Be("Admin");

            _repositoryMock.Verify(x =>
                x.Update(product),
                Times.Once);

            _unitOfWorkMock.Verify(x =>
                x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenProductNotFound()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Product?)null);

            var dto = new UpdateProductDto("Laptop");

            // Act
            Func<Task> act = async () =>
                await _service.UpdateAsync(1, dto, "Admin");

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act
            await _service.DeleteAsync(1);

            // Assert
            _repositoryMock.Verify(x =>
                x.Delete(product),
                Times.Once);

            _unitOfWorkMock.Verify(x =>
                x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenProductNotFound()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Product?)null);

            // Act
            Func<Task> act = async () =>
                await _service.DeleteAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedProducts()
        {
            // Arrange
            var pagination = new PaginationParams
            {
                Page = 1,
                PageSize = 10
            };

            var products = new List<Product>
            {
                new()
                {
                    Id = 1,
                    ProductName = "Laptop",
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.UtcNow
                }
            };

            var dtoList = new List<ProductDto>
            {
                new(
                    1,
                    "Laptop",
                    "Admin",
                    products[0].CreatedOn)
            };

            _repositoryMock
                .Setup(x => x.GetAllAsync(pagination))
                .ReturnsAsync((products, 1));

            _mapperMock
                .Setup(x => x.Map<IEnumerable<ProductDto>>(products))
                .Returns(dtoList);

            // Act
            var result = await _service.GetAllAsync(pagination);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(1);
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(10);

            _repositoryMock.Verify(x =>
                x.GetAllAsync(pagination),
                Times.Once);
        }

        #endregion
    }
}