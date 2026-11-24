using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudentDisciplineGrades.Queries;

public sealed record GetStudentDisciplineGradesQuery(
    string? Grade,
    int? StudentId,
    int? DisciplineId,
    int? EmployeeId,
    int? AcademicPerformanceId,
    int? AdmissionYear,
    int? SemesterNumber,
    DateTime? DateFrom,
    DateTime? DateTo) : IRequest<IReadOnlyList<StudentDisciplineGradeDto>>;

public sealed class GetStudentDisciplineGradesQueryHandler(IStudentDisciplineGradeRepository repository)
    : IRequestHandler<GetStudentDisciplineGradesQuery, IReadOnlyList<StudentDisciplineGradeDto>>
{
    public async Task<IReadOnlyList<StudentDisciplineGradeDto>> Handle(GetStudentDisciplineGradesQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<StudentDisciplineGrade> studentDisciplineGrades = await repository.GetAllAsync(
            request.Grade,
            request.StudentId,
            request.DisciplineId,
            request.EmployeeId,
            request.AcademicPerformanceId,
            request.AdmissionYear,
            request.SemesterNumber,
            request.DateFrom,
            request.DateTo,
            cancellationToken);

        return studentDisciplineGrades.Select(StudentDisciplineGradeDto.From).ToArray();
    }
}
