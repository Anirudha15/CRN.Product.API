using CRN.Product.API.Repositories;
using CRN.Product.API.Services;
using Moq;
using ProductEntity = CRN.Product.API.Entities.Product;

namespace CRN.Product.API.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _repositoryMock = new Mock<IProductRepository>();

            _productService =
                new ProductService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ProductExists_ReturnsProduct()
        {
            // Arrange
            var product = new ProductEntity
            {
                Id = 1,
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act
            var result =
                await _productService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Laptop", result.ProductName);

            _repositoryMock.Verify(
                r => r.GetByIdAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ProductDoesNotExist_ReturnsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((ProductEntity?)null);

            // Act
            var result =
                await _productService.GetByIdAsync(99);

            // Assert
            Assert.Null(result);

            _repositoryMock.Verify(
                r => r.GetByIdAsync(99),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Product_CreatesProduct()
        {
            // Arrange
            var product = new ProductEntity
            {
                ProductName = "Mobile",
                CreatedBy = "Admin"
            };

            _repositoryMock
                .Setup(r => r.CreateAsync(product))
                .ReturnsAsync(product);

            // Act
            var result =
                await _productService.CreateAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Mobile", result.ProductName);
            Assert.NotEqual(default, result.CreatedOn);

            _repositoryMock.Verify(
                r => r.CreateAsync(product),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ProductExists_ReturnsTrue()
        {
            // Arrange
            var existingProduct = new ProductEntity
            {
                Id = 1,
                ProductName = "Old Laptop",
                CreatedBy = "Admin"
            };

            var updatedProduct = new ProductEntity
            {
                ProductName = "New Laptop",
                CreatedBy = "Admin"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingProduct);

            _repositoryMock
                .Setup(r => r.UpdateAsync(existingProduct))
                .Returns(Task.CompletedTask);

            // Act
            var result =
                await _productService.UpdateAsync(
                    1,
                    updatedProduct);

            // Assert
            Assert.True(result);
            Assert.Equal(
                "New Laptop",
                existingProduct.ProductName);

            _repositoryMock.Verify(
                r => r.UpdateAsync(existingProduct),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ProductDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((ProductEntity?)null);

            var product = new ProductEntity
            {
                ProductName = "Laptop",
                CreatedBy = "Admin"
            };

            // Act
            var result =
                await _productService.UpdateAsync(
                    99,
                    product);

            // Assert
            Assert.False(result);

            _repositoryMock.Verify(
                r => r.UpdateAsync(
                    It.IsAny<ProductEntity>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ProductExists_ReturnsTrue()
        {
            // Arrange
            var product = new ProductEntity
            {
                Id = 1,
                ProductName = "Laptop",
                CreatedBy = "Admin"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(product);

            _repositoryMock
                .Setup(r => r.DeleteAsync(product))
                .Returns(Task.CompletedTask);

            // Act
            var result =
                await _productService.DeleteAsync(1);

            // Assert
            Assert.True(result);

            _repositoryMock.Verify(
                r => r.DeleteAsync(product),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ProductDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((ProductEntity?)null);

            // Act
            var result =
                await _productService.DeleteAsync(99);

            // Assert
            Assert.False(result);

            _repositoryMock.Verify(
                r => r.DeleteAsync(
                    It.IsAny<ProductEntity>()),
                Times.Never);
        }
    }
}