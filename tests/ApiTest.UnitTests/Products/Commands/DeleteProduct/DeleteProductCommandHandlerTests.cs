using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.Products.Commands.DeleteProduct;
using ApiTest.Domain.Entities;
using Moq;

namespace ApiTest.UnitTests.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly Mock<IProductService> _mockProductService;

        public DeleteProductCommandHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _mockProductService = new Mock<IProductService>();
        }

        [Fact]
        public async Task Handle_ProductExists_ReturnsSuccess()
        {
            // Arrange
            var product = new Product { ProductId = 1 };
            _mockContext.Setup(c => c.Products.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);

            var handler = new DeleteProductCommandHandler(_mockContext.Object, _mockProductService.Object);

            // Act
            var result = await handler.Handle(new DeleteProductCommand(1), CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            _mockProductService.Verify(s => s.DeleteProductAsync(1), Times.Once);
        }
    }
}