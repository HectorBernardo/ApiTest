using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.Inventory.Commands.CreateMovement;
using ApiTest.Domain.Entities;
using ApiTest.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTest.UnitTests.Inventory.Commands.CreateMovement
{
    public class CreateMovementCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        public CreateMovementCommandHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
        }

        [Fact]
        public async Task Handle_ValidMovement_UpdatesProductStockSuccessfully()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            
            var product = new Product
            {
                ProductId = 1,
                Name = "Mouse Gamer",
                Price = 25,
                Stock = 10,
                IsDeleted = false
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();
            
            var handler = new CreateMovementCommandHandler(context);

            var command = new CreateMovementCommand(1, 3, "Output", "Venta al cliente"); // Adjust according to the 4 parameters requested by your constructor

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(1);

            var updatedProduct = await context.Products.FindAsync(1);
            updatedProduct.Stock.Should().Be(7);
        }
    }
}
