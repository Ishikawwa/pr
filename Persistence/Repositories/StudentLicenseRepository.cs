using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class StudentLicenseRepository(AppDbContext context) : IStudentLicenseRepository
{
    public async Task<IReadOnlyList<StudentLicense>> GetAllAsync(int? studentId, int? licenseId, string? studentName, string? licenseName, CancellationToken cancellationToken)
    {
        IQueryable<StudentLicense> query = context.StudentLicenses
            .AsNoTracking()
            .Include(studentLicense => studentLicense.Student)
            .Include(studentLicense => studentLicense.License);

        if (studentId.HasValue)
            query = query.Where(studentLicense => studentLicense.StudentId == studentId.Value);

        if (licenseId.HasValue)
            query = query.Where(studentLicense => studentLicense.LicenseId == licenseId.Value);

        if (!string.IsNullOrWhiteSpace(studentName))
        {
            string searchPattern = $"%{studentName.Trim()}%";

            query = query.Where(studentLicense =>
                EF.Functions.ILike(studentLicense.Student.LastName, searchPattern) ||
                EF.Functions.ILike(studentLicense.Student.FirstName, searchPattern) ||
                (studentLicense.Student.Patronymic != null && EF.Functions.ILike(studentLicense.Student.Patronymic, searchPattern)));
        }

        if (!string.IsNullOrWhiteSpace(licenseName))
            query = query.Where(studentLicense => EF.Functions.ILike(studentLicense.License.Name, $"%{licenseName.Trim()}%"));

        return await query.OrderBy(studentLicense => studentLicense.Student.LastName)
            .ThenBy(studentLicense => studentLicense.Student.FirstName)
            .ThenBy(studentLicense => studentLicense.License.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<StudentLicense?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.StudentLicenses
            .Include(studentLicense => studentLicense.Student)
            .Include(studentLicense => studentLicense.License)
            .FirstOrDefaultAsync(studentLicense => studentLicense.Id == id, cancellationToken);

    public async Task AddAsync(StudentLicense studentLicense, CancellationToken cancellationToken)
        => await context.StudentLicenses.AddAsync(studentLicense, cancellationToken);

    public void Update(StudentLicense studentLicense)
        => context.StudentLicenses.Update(studentLicense);

    public void Delete(StudentLicense studentLicense)
        => context.StudentLicenses.Remove(studentLicense);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
