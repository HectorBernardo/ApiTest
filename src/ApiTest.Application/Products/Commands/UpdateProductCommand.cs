using ApiTest.Application.DTOs;
using ApiTest.Domain.Entities;
using MediatR;

namespace ApiTest.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    int ProductId,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    int CategoryId,
    bool IsDeleted = true
) : IRequest<CommandResponse>;