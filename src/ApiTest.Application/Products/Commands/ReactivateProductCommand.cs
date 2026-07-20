using MediatR;
using ApiTest.Application.DTOs;

namespace ApiTest.Application.Products.Commands.ReactivateProduct;

public record ReactivateProductCommand(int ProductId) : IRequest<CommandResponse>;