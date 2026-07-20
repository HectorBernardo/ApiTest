using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.Products.Queries.GetProductsList;
using ApiTest.Domain.Entities;
using ApiTest.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApiTest.UnitTests.Products.Queries.Get
{
    public class GetProductsListQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;

        public GetProductsListQueryHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
        }

        [Fact(Skip = "Pending configuration of EF Mock with DTO projections")]
        public async Task Handle_ReturnsAllProducts()
        {
            var data = new List<Product>
            {
                new Product { ProductId = 1, Name = "Producto 1" },
                new Product { ProductId = 2, Name = "Producto 2" }
            }.AsQueryable();
            
            var mockSet = new Mock<DbSet<Product>>();
            
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            var handler = new GetProductsListQueryHandler(_mockContext.Object);

            // Act
            var result = await handler.Handle(new GetProductsListQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}