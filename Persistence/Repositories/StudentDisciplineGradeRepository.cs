using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class StudentDisciplineGradeRepository(AppDbContext db) : IStudentDisciplineGradeRepository
{
    public async Task<IReadOnlyList<StudentDisciplineGrade>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await db.StudentDisciplineGrades
            .AsNoTracking()
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