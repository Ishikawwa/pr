using Application.Features.StudentDisciplineGrades;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

public sealed class StudentDisciplineGradeExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not StudentDisciplineGradeNotFoundException)
        {
            return false;
        }

        context.Response.StatusCode = StatusCodes.Status404NotFound;

        ProblemDetails problem = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Student discipline grade not found",
            Detail = exception.Message
        };

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}