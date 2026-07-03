using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.API.Controllers;

namespace ProductApi.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _serviceMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _serviceMock = new Mock<IProductService>();
            _controller = new ProductsController(_serviceMock.Object);

            var user = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(
                    new[]
                    {
                        new System.Security.Claims.Claim(
                            System.Security.Claims.ClaimTypes.Name,
                            "Admin")
                    },
                    "TestAuthentication"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _controller.ControllerContext.HttpContext.User = user;
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var pagination = new PaginationParams
            {
                Page = 1,
                PageSize = 10
            };

            var pagedResult = new PagedResultDto<ProductDto>(
                new List<ProductDto>(),
                0,
                1,
                10,
                0);

            _serviceMock
                .Setup(x => x.GetAllAsync(pagination))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetAll(pagination);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(pagedResult, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult()
        {
            // Arrange
            var dto = new ProductDto(
                1,
                "Laptop",
                "Admin",
                DateTime.UtcNow);

            _serviceMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(dto);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(dto, ok.Value);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = new CreateProductDto("Laptop");

            var dto = new ProductDto(
                1,
                "Laptop",
                "Admin",
                DateTime.UtcNow);

            _serviceMock
                .Setup(x => x.CreateAsync(createDto, "Admin"))
                .ReturnsAsync(dto);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(nameof(_controller.GetById), created.ActionName);

            Assert.Equal(dto, created.Value);
        }

        [Fact]
        public async Task Update_ReturnsNoContent()
        {
            // Arrange
            var dto = new UpdateProductDto("New Laptop");

            _serviceMock
                .Setup(x => x.UpdateAsync(1, dto, "Admin"))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            _serviceMock
                .Setup(x => x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}