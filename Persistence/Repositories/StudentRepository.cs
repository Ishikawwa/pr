using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class StudentRepository(AppDbContext db)
    : IStudentRepository
{
    public async Task<IReadOnlyList<Student>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await db.Students
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Student?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await db.Students
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(
        string email,
        int? excludingId,
        CancellationToken cancellationToken)
    {
        return await db.Students.AnyAsync(
            x => x.Email == email &&
                 (!excludingId.HasValue ||
                  x.Id != excludingId.Value),
            cancellationToken);
    }

    public async Task<Student> AddAsync(
        Student student,
        CancellationToken cancellationToken)
    {
        db.Students.Add(student);

        await db.SaveChangesAsync(cancellationToken);

        return student;
    }

    public async Task UpdateAsync(
        Student student,
        CancellationToken cancellationToken)
    {
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Student student,
        CancellationToken cancellationToken)
    {
        db.Students.Remove(student);

        await db.SaveChangesAsync(cancellationToken);
    }
}