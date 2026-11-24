using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Students.Commands;

public sealed record DeleteStudentCommand(int Id) : IRequest;

public sealed class DeleteStudentCommandHandler(
    IStudentRepository repository)
    : IRequestHandler<DeleteStudentCommand>
{
    public async Task Handle(
        DeleteStudentCommand request,
        CancellationToken cancellationToken)
    {
        var student = await repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (student is null)
        {
            throw new StudentNotFoundException(request.Id);
        }

        await repository.DeleteAsync(
            student,
            cancellationToken);
    }
}