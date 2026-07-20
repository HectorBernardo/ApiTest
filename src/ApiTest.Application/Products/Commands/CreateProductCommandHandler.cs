using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.DTOs;
using ApiTest.Domain.Entities;
using Dapper;
using MediatR;

namespace ApiTest.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CommandResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IProductService _productService;

    public CreateProductCommandHandler(IApplicationDbContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    public async Task<CommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        
        await _productService.CreateProductAsync(product);

        return new CommandResponse(true, "Producto creado correctamente", product.ProductId);
    }
}