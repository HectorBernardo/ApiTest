using MediatR;

public record GetCategoryByIdQuery(int CategoryId) : IRequest<CategoryDto?>;