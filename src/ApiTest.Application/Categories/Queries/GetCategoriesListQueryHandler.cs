using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Application.Categories.Queries.GetCategoriesList;

public class GetCategoriesListQueryHandler : IRequestHandler<GetCategoriesListQuery, List<CategoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoriesListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoriesListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .Select(c => new CategoryDto(c.CategoryId, c.Name, c.Description))
            .ToListAsync(cancellationToken);
    }
}