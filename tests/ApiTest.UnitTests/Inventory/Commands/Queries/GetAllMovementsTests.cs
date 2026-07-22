using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.Inventory.Queries;
using ApiTest.Domain.Entities;
using ApiTest.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTest.UnitTests.Inventory.Commands.Queries
{
    public class GetAllMovementsTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;

        public GetAllMovementsTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
        }

        [Fact]
        public async Task Handle_ReturnsAllMovements()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            context.Products.Add(new Product
            {
                ProductId = 3,
                Name = "Teclado iluminado mecánico",
                Price = 100,
                Stock = 10
            });

            
            context.InventoryMovements.Add(new InventoryMovement
            {
                MovementId = 1,
                ProductId = 3,
                Quantity = 1,
                MovementType = "Output",
                Reason = "Venta al cliente final",
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var handler = new GetMovementsListQueryHandler(context);

            // Act
            var result = await handler.Handle(new GetMovementsListQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }
    }
}
