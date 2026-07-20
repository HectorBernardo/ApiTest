using MediatR;

public record GetCategoriesListQuery : IRequest<List<CategoryDto>>;