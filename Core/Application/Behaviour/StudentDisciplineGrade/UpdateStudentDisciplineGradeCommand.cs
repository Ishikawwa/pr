using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudentDisciplineGrades.Commands;

public sealed record UpdateStudentDisciplineGradeCommand(
    int Id,
    int StudentId,
    int DisciplineId,
    int EmployeeId,
    int? AcademicPerformanceId,
    int AdmissionYear,
    int SemesterNumber,
    string Grade,
    DateTime Date
) : IRequest<StudentDisciplineGradeDto>;

public sealed class UpdateStudentDisciplineGradeCommandHandler(IStudentDisciplineGradeRepository repository)
    : IRequestHandler<UpdateStudentDisciplineGradeCommand, StudentDisciplineGradeDto>
{
    public async Task<StudentDisciplineGradeDto> Handle(UpdateStudentDisciplineGradeCommand request, CancellationToken cancellationToken)
    {
        StudentDisciplineGrade? studentDisciplineGrade = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studentDisciplineGrade is null)
        {
            throw new StudentDisciplineGradeNotFoundException(request.Id);
        }

        studentDisciplineGrade.StudentId = request.StudentId;
        studentDisciplineGrade.DisciplineId = request.DisciplineId;
        studentDisciplineGrade.EmployeeId = request.EmployeeId;
        studentDisciplineGrade.AcademicPerformanceId = request.AcademicPerformanceId;
        studentDisciplineGrade.AdmissionYear = request.AdmissionYear;
        studentDisciplineGrade.SemesterNumber = request.SemesterNumber;
        studentDisciplineGrade.Grade = request.Grade;
        studentDisciplineGrade.Date = request.Date;

        await repository.UpdateAsync(studentDisciplineGrade, cancellationToken);

        return StudentDisciplineGradeDto.From(studentDisciplineGrade);
    }
}