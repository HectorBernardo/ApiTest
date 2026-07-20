using MediatR;
using ApiTest.Application.DTOs;

namespace ApiTest.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(int ProductId) : IRequest<ProductDetailDto>;