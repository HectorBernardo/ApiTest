using MediatR;
using ApiTest.Application.DTOs; // Donde tienes tu CommandResponse

namespace ApiTest.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(int ProductId) : IRequest<CommandResponse>;