using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.DTOs;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, CommandResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IProductService _productService;

    public UpdateProductCommandHandler(IApplicationDbContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    public async Task<CommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);

        if (product == null)
            return new CommandResponse(false, "Producto no encontrado", null);

        product.Name = request.Name;
        product.Price = request.Price;

        await _productService.UpdateProductAsync(product);

        return new CommandResponse(true, "Actualizado correctamente", null);
    }
}