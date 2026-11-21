using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudentDisciplineGrades.Queries;

public sealed record GetStudentDisciplineGradesQuery : IRequest<IReadOnlyList<StudentDisciplineGradeDto>>;

public sealed class GetStudentDisciplineGradesQueryHandler(IStudentDisciplineGradeRepository repository)
    : IRequestHandler<GetStudentDisciplineGradesQuery, IReadOnlyList<StudentDisciplineGradeDto>>
{
    public async Task<IReadOnlyList<StudentDisciplineGradeDto>> Handle(GetStudentDisciplineGradesQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<StudentDisciplineGrade> studentDisciplineGrades = await repository.GetAllAsync(cancellationToken);

        return studentDisciplineGrades.Select(StudentDisciplineGradeDto.From).ToArray();
    }
}