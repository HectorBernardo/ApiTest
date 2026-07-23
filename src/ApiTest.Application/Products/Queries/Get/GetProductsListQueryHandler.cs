using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.DTOs;
using ApiTest.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Application.Products.Queries.GetProductsList;

public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, List<ProductResponseDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductResponseDto>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products
        .Include(p => p.Category)
        .AsNoTracking()
        .Select(p => new ProductResponseDto
        {
            ProductId = p.ProductId,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            CategoryId = p.CategoryId,
            CreatedAt = p.CreatedAt,
            Category = p.Category != null ? new CategoryResponseDto
            {
                CategoryId = p.Category.CategoryId,
                Name = p.Category.Name,
                Description = p.Category.Description
            } : null,
            IsDeleted = p.IsDeleted
        })
         .ToListAsync(cancellationToken);
    }
}