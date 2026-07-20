using MediatR;
using ApiTest.Application.DTOs;

namespace ApiTest.Application.Categories.Commands;

public record UpdateCategoryCommand(int CategoryId, string Name, string Description) : IRequest<CommandResponse>;