using ApiTest.Application.DTOs;
using MediatR;

namespace ApiTest.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    int ProductId,
    string Name,
    string Description,
    decimal Price
) : IRequest<CommandResponse>;