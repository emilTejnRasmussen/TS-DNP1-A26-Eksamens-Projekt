using Microsoft.AspNetCore.Diagnostics;
using RepositoryContract.Exceptions;

namespace WebApi.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is EntityNotFoundException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            await httpContext.Response.WriteAsJsonAsync(
                new
                {
                    error = exception.Message
                },
                cancellationToken
            );

            return true;
        }

        return false;
    }
}