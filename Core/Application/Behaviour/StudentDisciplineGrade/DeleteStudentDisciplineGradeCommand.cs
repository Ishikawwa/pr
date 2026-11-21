using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudentDisciplineGrades.Commands;

public sealed record DeleteStudentDisciplineGradeCommand(int Id) : IRequest;

public sealed class DeleteStudentDisciplineGradeCommandHandler(IStudentDisciplineGradeRepository repository)
    : IRequestHandler<DeleteStudentDisciplineGradeCommand>
{
    public async Task Handle(DeleteStudentDisciplineGradeCommand request, CancellationToken cancellationToken)
    {
        StudentDisciplineGrade? studentDisciplineGrade = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studentDisciplineGrade is null)
        {
            throw new StudentDisciplineGradeNotFoundException(request.Id);
        }

        await repository.DeleteAsync(studentDisciplineGrade, cancellationToken);
    }
}