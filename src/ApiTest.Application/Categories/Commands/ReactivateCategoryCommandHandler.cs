using ApiTest.Application.Common.Interfaces;
using ApiTest.Application.DTOs;
using Dapper;
using MediatR;

namespace ApiTest.Application.Categories.Commands.ReactivateCategory
{
    public class ReactivateCategoryCommandHandler : IRequestHandler<ReactivateCategoryCommand, CommandResponse>
    {
        private readonly IApplicationDbContext _context;

        public ReactivateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse> Handle(ReactivateCategoryCommand request, CancellationToken cancellationToken)
        {
            
            var category = await _context.Categories.FindAsync(new object[] { request.CategoryId }, cancellationToken);

            if (category == null)
                return new CommandResponse(false, "Categoría no encontrada", null);

            if (category.IsDeleted)
                return new CommandResponse(false, "La categoría ya se encuentra activa", null);

            
            const string sql = "UPDATE Categories SET IsDeleted = 1 WHERE CategoryId = @CategoryId";

            await _context.Connection.ExecuteAsync(sql, new { request.CategoryId });

            return new CommandResponse(true, "Categoría reactivada correctamente", null);
        }
    }
}