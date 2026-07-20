using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.Products.Commands.CreateProduct;
using ApiTest.Domain.Entities;
using Moq;


namespace ApiTest.UnitTests.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly Mock<IProductService> _mockProductService;

        public CreateProductCommandHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _mockProductService = new Mock<IProductService>();
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesProductAndReturnsSuccess()
        {
            // Arrange
            var command = new CreateProductCommand("Nuevo Producto","Descripción de prueba",15.0m,10,1);
            var handler = new CreateProductCommandHandler(_mockContext.Object, _mockProductService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Producto creado correctamente", result.Message);

            
            _mockProductService.Verify(s => s.CreateProductAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}
