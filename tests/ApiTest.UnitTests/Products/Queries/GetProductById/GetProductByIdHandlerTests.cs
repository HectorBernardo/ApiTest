using ApiTest.Application.Categories.Queries.GetCategoryById;
using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.Products.Queries.GetProductById;
using ApiTest.Domain.Entities;
using ApiTest.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTest.UnitTests.Products.Queries.GetProductById
{
    public class GetProductByIdHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;

        public GetProductByIdHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
        }

        [Fact]
        public async Task Handle_ReturnsProductById_WhenExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            var category = new Category { CategoryId = 1, Name = "Electrónica" };
            context.Categories.Add(category);

            context.Products.Add(new Product
            {
                ProductId = 10,
                Name = "Teclado",
                Price = 50,
                Stock = 10,
                CategoryId = 1,
                Category = category
            });

            await context.SaveChangesAsync();

            var handler = new GetProductByIdQueryHandler(context); // Ajusta al nombre real de tu handler

            // Act
            var result = await handler.Handle(new GetProductByIdQuery(10), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ProductId.Should().Be(10);
            result.Name.Should().Be("Teclado");
        }
    }
}
