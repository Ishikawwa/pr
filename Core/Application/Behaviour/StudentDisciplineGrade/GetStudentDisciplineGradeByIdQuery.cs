using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudentDisciplineGrades.Queries;

public sealed record GetStudentDisciplineGradeByIdQuery(int Id) : IRequest<StudentDisciplineGradeDto>;

public sealed class GetStudentDisciplineGradeByIdQueryHandler(IStudentDisciplineGradeRepository repository)
    : IRequestHandler<GetStudentDisciplineGradeByIdQuery, StudentDisciplineGradeDto>
{
    public async Task<StudentDisciplineGradeDto> Handle(GetStudentDisciplineGradeByIdQuery request, CancellationToken cancellationToken)
    {
        StudentDisciplineGrade? studentDisciplineGrade = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studentDisciplineGrade is null)
        {
            throw new StudentDisciplineGradeNotFoundException(request.Id);
        }

        return StudentDisciplineGradeDto.From(studentDisciplineGrade);
    }
}