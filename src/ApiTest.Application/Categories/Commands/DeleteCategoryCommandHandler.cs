using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.DTOs;
using Dapper;
using MediatR;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, CommandResponse>
{
    private readonly IApplicationDbContext _context;
    public DeleteCategoryCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<CommandResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var connection = _context.Connection;

        const string sql = "UPDATE Categories SET IsDeleted = 0 WHERE CategoryId = @CategoryId";

        await connection.ExecuteAsync(sql, new { request.CategoryId });

        return new CommandResponse(true, "Operación realizada con éxito", null);
    }
}