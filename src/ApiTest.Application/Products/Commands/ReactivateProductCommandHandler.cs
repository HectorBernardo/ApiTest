using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.DTOs;
using Dapper;
using MediatR;

namespace ApiTest.Application.Products.Commands.ReactivateProduct;

public class ReactivateProductCommandHandler : IRequestHandler<ReactivateProductCommand, CommandResponse>
{
    private readonly IApplicationDbContext _context;

    public ReactivateProductCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<CommandResponse> Handle(ReactivateProductCommand request, CancellationToken cancellationToken)
    {
        
        var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);

        if (product == null) return new CommandResponse(false, "Producto no encontrado", null);
        if (!product.IsDeleted) return new CommandResponse(false, "El producto ya se encuentra activo", null);

        
        const string sql = "UPDATE Products SET IsDeleted = 1 WHERE ProductId = @ProductId";
        await _context.Connection.ExecuteAsync(sql, new { request.ProductId });

        return new CommandResponse(true, "Producto reactivado correctamente", null);
    }
}