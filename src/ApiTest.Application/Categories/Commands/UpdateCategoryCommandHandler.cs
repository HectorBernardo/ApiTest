using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.DTOs;
using Dapper;
using MediatR;

namespace ApiTest.Application.Categories.Commands;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CommandResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CommandResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var connection = _context.Connection;

        const string sql = @"
        UPDATE Categories 
        SET Name = @Name 
        WHERE CategoryId = @CategoryId";

        // Dapper maps the properties of the 'request' object to the @Name and @CategoryId parameters
        await connection.ExecuteAsync(sql, new
        {
            request.Name,
            request.CategoryId
        });

        
        return new CommandResponse(true, "Operación realizada con éxito", null);
    }
}