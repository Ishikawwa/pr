using Application.Features.AcademicPerformanceRecords;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

public sealed class AcademicPerformanceRecordExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int status = exception switch
        {
            AcademicPerformanceRecordNotFoundException => StatusCodes.Status404NotFound,
            AcademicPerformanceRecordConflictException => StatusCodes.Status409Conflict,
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
                404 => "Academic performance record not found",
                _ => "Academic performance record conflict"
            },
            Detail = exception.Message
        };

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}