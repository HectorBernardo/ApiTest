using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.DTOs;
using MediatR;

namespace ApiTest.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, CommandResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IProductService _productService;

    public DeleteProductCommandHandler(IApplicationDbContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    public async Task<CommandResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        
        var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);

        if (product == null || product.IsDeleted)
            return new CommandResponse(false, "Producto no encontrado o ya eliminado", null);

        
        await _productService.DeleteProductAsync(request.ProductId);

        return new CommandResponse(true, "Producto eliminado correctamente", null);
    }
}