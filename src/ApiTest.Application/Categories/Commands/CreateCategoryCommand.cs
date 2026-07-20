using MediatR;

namespace ApiTest.Application.Categories.Commands;

public record CreateCategoryCommand(string Name) : IRequest<int>;