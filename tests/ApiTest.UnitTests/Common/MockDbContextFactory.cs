using Moq;
using Microsoft.EntityFrameworkCore;
using ApiTest.Application.Common.Interfaces;
using ApiTest.Domain.Entities;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ApiTest.UnitTests.Common
{
    public static class MockDbContextFactory
    {
        public static Mock<IApplicationDbContext> Create()
        {
            var mock = new Mock<IApplicationDbContext>();

            
            var data = new List<Product>().AsQueryable();
            var mockSet = new Mock<DbSet<Product>>();

            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mock.Setup(c => c.Products).Returns(mockSet.Object);

            
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();
            mock.Setup(c => c.Connection).Returns(mockConnection.Object);

            
            mockCommand.SetupAllProperties();

            
            mock.Setup(c => c.Connection).Returns(mockConnection.Object);

            return mock;
        }
    }
}