using Domain.Entities;

namespace Application.Features.StudentDisciplineGrades;

public sealed record StudentDisciplineGradeDto(
    int Id,
    int StudentId,
    int DisciplineId,
    int EmployeeId,
    int? AcademicPerformanceId,
    int AdmissionYear,
    int SemesterNumber,
    string Grade,
    DateTime Date)
{
    public static StudentDisciplineGradeDto From(StudentDisciplineGrade studentDisciplineGrade)
    {
        return new StudentDisciplineGradeDto(
            studentDisciplineGrade.Id,
            studentDisciplineGrade.StudentId,
            studentDisciplineGrade.DisciplineId,
            studentDisciplineGrade.EmployeeId,
            studentDisciplineGrade.AcademicPerformanceId,
            studentDisciplineGrade.AdmissionYear,
            studentDisciplineGrade.SemesterNumber,
            studentDisciplineGrade.Grade,
            studentDisciplineGrade.Date
        );
    }
}