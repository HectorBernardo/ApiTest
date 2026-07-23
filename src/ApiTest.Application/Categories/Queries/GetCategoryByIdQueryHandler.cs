using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CategoryId == request.CategoryId, cancellationToken);

        return category == null
            ? null
            : new CategoryDto(category.CategoryId, category.Name, category.Description, category.IsDeleted);
    }
}