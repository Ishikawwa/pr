using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudentDisciplineGrades.Commands;

public sealed record CreateStudentDisciplineGradeCommand(
    int StudentId,
    int DisciplineId,
    int EmployeeId,
    int? AcademicPerformanceId,
    int AdmissionYear,
    int SemesterNumber,
    string Grade,
    DateTime Date
) : IRequest<StudentDisciplineGradeDto>;

public sealed class CreateStudentDisciplineGradeCommandHandler(IStudentDisciplineGradeRepository repository)
    : IRequestHandler<CreateStudentDisciplineGradeCommand, StudentDisciplineGradeDto>
{
    public async Task<StudentDisciplineGradeDto> Handle(CreateStudentDisciplineGradeCommand request, CancellationToken cancellationToken)
    {
        StudentDisciplineGrade studentDisciplineGrade = new StudentDisciplineGrade
        {
            StudentId = request.StudentId,
            DisciplineId = request.DisciplineId,
            EmployeeId = request.EmployeeId,
            AcademicPerformanceId = request.AcademicPerformanceId,
            AdmissionYear = request.AdmissionYear,
            SemesterNumber = request.SemesterNumber,
            Grade = request.Grade,
            Date = request.Date
        };

        StudentDisciplineGrade createdStudentDisciplineGrade = await repository.AddAsync(studentDisciplineGrade, cancellationToken);

        return StudentDisciplineGradeDto.From(createdStudentDisciplineGrade);
    }
}