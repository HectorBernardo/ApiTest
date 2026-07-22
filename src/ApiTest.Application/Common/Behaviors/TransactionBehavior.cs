using MediatR;
using Microsoft.EntityFrameworkCore;
using ApiTest.Application.Common.Interfaces;

namespace ApiTest.Application.Common.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IApplicationDbContext _dbContext;

        public TransactionBehavior(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // If the request is NOT a command (i.e., it's a read query),
            // we let it pass directly without opening an unnecessary transaction to the database.

            string requestName = typeof(TRequest).Name;
            if (!requestName.EndsWith("Command"))
            {
                return await next();
            }

            // If it is a Command (Create, Update, Delete, etc.), we execute it under a secure transactional strategy
            var strategy = (_dbContext as DbContext)?.Database.CreateExecutionStrategy();

            if (strategy == null)
            {
                // Fallback in case the context doesn't directly explain the strategy
                await _dbContext.BeginTransactionAsync(cancellationToken);
                try
                {
                    var response = await next();
                    await _dbContext.CommitTransactionAsync(cancellationToken);
                    return response;
                }
                catch
                {
                    await _dbContext.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }

            return await strategy.ExecuteAsync(async () =>
            {
                await _dbContext.BeginTransactionAsync(cancellationToken);
                try
                {
                    var response = await next();
                    await _dbContext.CommitTransactionAsync(cancellationToken);
                    return response;
                }
                catch
                {
                    await _dbContext.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            });
        }
    }
}