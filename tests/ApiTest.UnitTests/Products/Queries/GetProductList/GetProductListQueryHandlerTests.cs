using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.Products.Queries.GetProductsList;
using ApiTest.Domain.Entities;
using ApiTest.Infrastructure.Persistence;
using ApiTest.UnitTests.Helpers;
using FluentAssertions;
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

        [Fact]
        public async Task Handle_ReturnsAllProducts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            // Sembramos datos de prueba incluyendo la categoría para que el .Include() y el DTO funcionen perfecto
            var category = new Category { CategoryId = 1, Name = "Electrónica", Description = "Artículos electrónicos" };
            context.Categories.Add(category);

            context.Products.Add(new Product
            {
                ProductId = 1,
                Name = "Laptop",
                Description = "Gamer",
                Price = 1500,
                Stock = 5,
                CategoryId = 1,
                IsDeleted = false,
                Category = category
            });

            await context.SaveChangesAsync();

            var handler = new GetProductsListQueryHandler(context);

            // Act
            var result = await handler.Handle(new GetProductsListQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().Contain(p => p.Name == "Laptop");
        }
    }
}