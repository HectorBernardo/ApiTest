using MediatR;
using ApiTest.Application.Common.Interfaces;
using Dapper;

namespace ApiTest.Application.Categories.Commands;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var connection = _context.Connection;

        const string sql = @"
        INSERT INTO Categories (Name) 
        VALUES (@Name);
        SELECT CAST(SCOPE_IDENTITY() as int);";

        var newId = await connection.QuerySingleAsync<int>(sql, new { Name = request.Name });

        return newId;
    }
}