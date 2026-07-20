using ApiTest.Application.DTOs;
using MediatR;

public record DeleteCategoryCommand(int CategoryId) : IRequest<CommandResponse>;