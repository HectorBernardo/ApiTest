using ApiTest.Application.Common.Interfaces;
using ApiTest.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Application.Inventory.Commands.CreateMovement;

public class CreateMovementCommandHandler : IRequestHandler<CreateMovementCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateMovementCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<int> Handle(CreateMovementCommand request, CancellationToken cancellationToken)
    {
        
        var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);
        if (product == null) throw new Exception("Producto no encontrado");

        if (request.MovementType == "Output" && (product.Stock - request.Quantity) < 0)
        {
            throw new InvalidOperationException($"Stock insuficiente. Stock actual: {product.Stock}");
        }

        
        if (request.MovementType == "Output")
        {
            product.Stock -= request.Quantity;
        }
        else // "Input"
        {
            product.Stock += request.Quantity;
        }

        
        var movement = new InventoryMovement
        {
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            MovementType = request.MovementType,
            Reason = request.Reason,
            CreatedAt = DateTime.UtcNow
        };

        _context.InventoryMovements.Add(movement);
        await _context.SaveChangesAsync(cancellationToken);

        return movement.MovementId;
    }
}