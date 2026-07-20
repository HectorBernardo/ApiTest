using MediatR;
using ApiTest.Application.DTOs;

namespace ApiTest.Application.Categories.Commands.ReactivateCategory
{
    public record ReactivateCategoryCommand(int CategoryId) : IRequest<CommandResponse>;
}