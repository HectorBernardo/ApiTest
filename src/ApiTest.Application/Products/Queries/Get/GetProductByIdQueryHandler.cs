using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<ProductDetailDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(p => p.InventoryMovements)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductId == request.ProductId, cancellationToken);

        if (product == null) return null; 

        return new ProductDetailDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CreatedAt = product.CreatedAt,
            InventoryMovements = product.InventoryMovements.Select(m => new InventoryMovementDto
            {
                MovementId = m.MovementId,
                Quantity = m.Quantity,
                MovementType = m.MovementType,
                Reason = m.Reason,
                CreatedAt = m.CreatedAt
            }).ToList()
        };
    }
}