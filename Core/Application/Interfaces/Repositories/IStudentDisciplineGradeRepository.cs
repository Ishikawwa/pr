using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IStudentDisciplineGradeRepository
{
    Task<IReadOnlyList<StudentDisciplineGrade>> GetAllAsync(
        string? grade,
        int? studentId,
        int? disciplineId,
        int? employeeId,
        int? academicPerformanceId,
        int? admissionYear,
        int? semesterNumber,
        DateTime? dateFrom,
        DateTime? dateTo,
        CancellationToken cancellationToken);

    Task<StudentDisciplineGrade?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<StudentDisciplineGrade> AddAsync(StudentDisciplineGrade studentDisciplineGrade, CancellationToken cancellationToken);

    Task UpdateAsync(StudentDisciplineGrade studentDisciplineGrade, CancellationToken cancellationToken);

    Task DeleteAsync(StudentDisciplineGrade studentDisciplineGrade, CancellationToken cancellationToken);
}
