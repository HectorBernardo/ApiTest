using FluentValidation;
using Microsoft.AspNetCore.Mvc;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try { await _next(context); }
        catch (ValidationException ex) // This is the exception that our Behavior throws at us.
        {
            context.Response.StatusCode = 400;
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            await context.Response.WriteAsJsonAsync(new { Errors = errors });
        }
    }
}