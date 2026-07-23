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
                            INSERT INTO Categories (Name, Description, IsDeleted)
                            VALUES (@Name, @Description, 1);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

        var newId = await connection.QuerySingleAsync<int>(sql, new
        {
            Name = request.Name,
            Description = request.Description
        });

        return newId;
    }
}