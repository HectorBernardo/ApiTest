using FluentValidation;
using MediatR;

namespace ApiTest.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                // Run all validators found for this command
                var failures = _validators
                    .Select(v => v.Validate(context))
                    .SelectMany(result => result.Errors)
                    .Where(f => f != null)
                    .ToList();

                // If there are errors, throw an exception (which in API will then handle as a 400 Bad Request)
                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }

            // If everything is okay, continue to the next step (the Handler)
            return await next();
        }
    }
}