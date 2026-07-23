using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Application.Inventory.Queries
{
    public class GetMovementsListQueryHandler : IRequestHandler<GetMovementsListQuery, List<MovementResponseDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetMovementsListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovementResponseDto>> Handle(GetMovementsListQuery request, CancellationToken cancellationToken)
        {
            return await _context.InventoryMovements
                .Include(m => m.Product)
                .AsNoTracking()
                .Select(m => new MovementResponseDto
                {
                    MovementId = m.MovementId,
                    ProductId = m.ProductId,
                    ProductName = m.Product != null ? m.Product.Name : string.Empty,
                    Quantity = m.Quantity,
                    MovementType = m.MovementType,
                    Reason = m.Reason,
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}
