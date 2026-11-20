using Application.Features.Students;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace WebApi;

public sealed class StudentExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var status = exception switch
        {
            ValidationException =>
                StatusCodes.Status400BadRequest,

            StudentNotFoundException =>
                StatusCodes.Status404NotFound,

            StudentEmailConflictException =>
                StatusCodes.Status409Conflict,

            DbUpdateException
            {
                InnerException: PostgresException
                {
                    SqlState: PostgresErrorCodes.UniqueViolation
                }
            } => StatusCodes.Status409Conflict,

            DbUpdateException
            {
                InnerException: PostgresException
                {
                    SqlState: PostgresErrorCodes.ForeignKeyViolation
                }
            } => StatusCodes.Status409Conflict,

            _ => 0
        };

        if (status == 0)
        {
            return false;
        }

        context.Response.StatusCode = status;

        var problem = new ProblemDetails
        {
            Status = status,
            Title = status switch
            {
                400 => "Validation failed",
                404 => "Student not found",
                _ => "Conflict"
            },
            Detail = status switch
            {
                400 => "Student data validation failed.",
                404 => "Student was not found.",
                _ => "The operation conflicts with existing data."
            }
        };

        if (exception is ValidationException validation)
        {
            problem.Extensions["errors"] = validation.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(e => e.ErrorMessage).ToArray());
        }

        await context.Response.WriteAsJsonAsync(
            problem,
            cancellationToken);

        return true;
    }
}