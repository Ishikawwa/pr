using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class StudentDisciplineGradeRepository(AppDbContext db) : IStudentDisciplineGradeRepository
{
    public async Task<IReadOnlyList<StudentDisciplineGrade>> GetAllAsync(
        string? grade,
        int? studentId,
        int? disciplineId,
        int? employeeId,
        int? academicPerformanceId,
        int? admissionYear,
        int? semesterNumber,
        DateTime? dateFrom,
        DateTime? dateTo,
        CancellationToken cancellationToken)
    {
        IQueryable<StudentDisciplineGrade> query = db.StudentDisciplineGrades.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(grade))
            query = query.Where(x => EF.Functions.ILike(x.Grade, $"%{grade.Trim()}%"));

        if (studentId.HasValue)
            query = query.Where(x => x.StudentId == studentId.Value);

        if (disciplineId.HasValue)
            query = query.Where(x => x.DisciplineId == disciplineId.Value);

        if (employeeId.HasValue)
            query = query.Where(x => x.EmployeeId == employeeId.Value);

        if (academicPerformanceId.HasValue)
            query = query.Where(x => x.AcademicPerformanceId == academicPerformanceId.Value);

        if (admissionYear.HasValue)
            query = query.Where(x => x.AdmissionYear == admissionYear.Value);

        if (semesterNumber.HasValue)
            query = query.Where(x => x.SemesterNumber == semesterNumber.Value);

        if (dateFrom.HasValue)
            query = query.Where(x => x.Date >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(x => x.Date <= dateTo.Value);

        return await query
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<StudentDisciplineGrade?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await db.StudentDisciplineGrades
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<StudentDisciplineGrade> AddAsync(StudentDisciplineGrade studentDisciplineGrade, CancellationToken cancellationToken)
    {
        db.StudentDisciplineGrades.Add(studentDisciplineGrade);

        await db.SaveChangesAsync(cancellationToken);

        return studentDisciplineGrade;
    }

    public async Task UpdateAsync(StudentDisciplineGrade studentDisciplineGrade, CancellationToken cancellationToken)
    {
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(StudentDisciplineGrade studentDisciplineGrade, CancellationToken cancellationToken)
    {
        db.StudentDisciplineGrades.Remove(studentDisciplineGrade);

        await db.SaveChangesAsync(cancellationToken);
    }
}
