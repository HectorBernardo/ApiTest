using ApiTest.Application.DTOs;
using ApiTest.Domain.Entities;
using MediatR;

namespace ApiTest.Application.Products.Queries.GetProductsList;


public record GetProductsListQuery() : IRequest<List<ProductResponseDto>>;