using ApiTest.Application.DTOs;
using MediatR;

namespace ApiTest.Application.Products.Commands.CreateProduct;


public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    int CategoryId
) : IRequest<CommandResponse>;