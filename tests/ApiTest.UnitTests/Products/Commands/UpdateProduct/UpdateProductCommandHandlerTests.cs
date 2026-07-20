using Moq;
using ApiTest.Application.Products.Commands.UpdateProduct;
using ApiTest.Application.Common.Interfaces;
using ApiTest.Domain.Entities;
using ApiTest.UnitTests.Common;

namespace ApiTest.UnitTests.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly Mock<IProductService> _mockProductService;

        public UpdateProductCommandHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _mockProductService = new Mock<IProductService>();
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesProductAndReturnsSuccess()
        {
            // Arrange
            var existingProduct = new Product { ProductId = 1, Name = "Producto Original", Price = 10.0m };
            var command = new UpdateProductCommand(1, "Producto Actualizado", "Alguna descripción", 20.0m);

            _mockContext.Setup(c => c.Products.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(existingProduct);

            var handler = new UpdateProductCommandHandler(_mockContext.Object, _mockProductService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Actualizado correctamente", result.Message);

            
            Assert.Equal(command.Name, existingProduct.Name);
            Assert.Equal(command.Price, existingProduct.Price);

            
            _mockProductService.Verify(s => s.UpdateProductAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ReturnsFailureResponse()
        {
            // Arrange
            var command = new UpdateProductCommand(999, "No existe", "", 0m);

            _mockContext.Setup(c => c.Products.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Product)null);

            var handler = new UpdateProductCommandHandler(_mockContext.Object, _mockProductService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Producto no encontrado", result.Message);

            _mockProductService.Verify(s => s.UpdateProductAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}