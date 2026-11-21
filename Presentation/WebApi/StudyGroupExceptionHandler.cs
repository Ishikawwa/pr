using Application.Features.StudyGroups;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

public sealed class StudyGroupExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int status = exception switch
        {
            StudyGroupNotFoundException => StatusCodes.Status404NotFound,
            StudyGroupNameConflictException => StatusCodes.Status409Conflict,
            _ => 0
        };

        if (status == 0)
        {
            return false;
        }

        context.Response.StatusCode = status;

        ProblemDetails problem = new ProblemDetails
        {
            Status = status,
            Title = status switch
            {
                404 => "Study group not found",
                _ => "Study group conflict"
            },
            Detail = exception.Message
        };

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}