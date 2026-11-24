
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class StudentRepository(AppDbContext db) : IStudentRepository
{
    public async Task<IReadOnlyList<Student>> GetAllAsync(string? search, int? admissionBase, int? admissionYear, CancellationToken cancellationToken)
    {
        IQueryable<Student> query = db.Students.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(student =>
                EF.Functions.ILike(student.Email, $"%{search.Trim()}%") ||
                EF.Functions.ILike(student.FirstName, $"%{search.Trim()}%") ||
                EF.Functions.ILike(student.LastName, $"%{search.Trim()}%") ||
                (student.Patronymic != null && EF.Functions.ILike(student.Patronymic, $"%{search.Trim()}%")));

        if (admissionBase.HasValue)
            query = query.Where(student => student.AdmissionBase == admissionBase.Value);

        if (admissionYear.HasValue)
            query = query.Where(student => student.AdmissionYear == admissionYear.Value);

        return await query.OrderBy(student => student.Id).ToListAsync(cancellationToken);
    }

    public async Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await db.Students.FirstOrDefaultAsync(student => student.Id == id, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludingId, CancellationToken cancellationToken)
    {
        return await db.Students.AnyAsync(
            student => student.Email == email &&
                       (!excludingId.HasValue || student.Id != excludingId.Value),
            cancellationToken);
    }

    public async Task<Student> AddAsync(Student student, CancellationToken cancellationToken)
    {
        db.Students.Add(student);

        await db.SaveChangesAsync(cancellationToken);

        return student;
    }

    public async Task UpdateAsync(Student student, CancellationToken cancellationToken)
    {
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Student student, CancellationToken cancellationToken)
    {
        db.Students.Remove(student);

        await db.SaveChangesAsync(cancellationToken);
    }
}
