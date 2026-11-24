using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudentDisciplineGrades.Commands;

public sealed record PatchStudentDisciplineGradeCommand(
    int Id,
    int? StudentId,
    int? DisciplineId,
    int? EmployeeId,
    int? AcademicPerformanceId,
    int? AdmissionYear,
    int? SemesterNumber,
    string? Grade,
    DateTime? Date) : IRequest<StudentDisciplineGradeDto>;

public sealed class PatchStudentDisciplineGradeCommandHandler(IStudentDisciplineGradeRepository repository)
    : IRequestHandler<PatchStudentDisciplineGradeCommand, StudentDisciplineGradeDto>
{
    public async Task<StudentDisciplineGradeDto> Handle(PatchStudentDisciplineGradeCommand request, CancellationToken cancellationToken)
    {
        StudentDisciplineGrade studentDisciplineGrade = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studentDisciplineGrade is null)
            throw new StudentDisciplineGradeNotFoundException(request.Id);

        if (request.StudentId.HasValue)
            studentDisciplineGrade.StudentId = request.StudentId.Value;

        if (request.DisciplineId.HasValue)
            studentDisciplineGrade.DisciplineId = request.DisciplineId.Value;

        if (request.EmployeeId.HasValue)
            studentDisciplineGrade.EmployeeId = request.EmployeeId.Value;

        if (request.AcademicPerformanceId.HasValue)
            studentDisciplineGrade.AcademicPerformanceId = request.AcademicPerformanceId.Value;

        if (request.AdmissionYear.HasValue)
            studentDisciplineGrade.AdmissionYear = request.AdmissionYear.Value;

        if (request.SemesterNumber.HasValue)
            studentDisciplineGrade.SemesterNumber = request.SemesterNumber.Value;

        if (request.Grade is not null)
            studentDisciplineGrade.Grade = request.Grade.Trim();

        if (request.Date.HasValue)
            studentDisciplineGrade.Date = request.Date.Value;

        await repository.UpdateAsync(studentDisciplineGrade, cancellationToken);

        return StudentDisciplineGradeDto.From(studentDisciplineGrade);
    }
}
