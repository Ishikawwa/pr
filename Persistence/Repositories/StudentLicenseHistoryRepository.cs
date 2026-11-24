using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class StudentLicenseHistoryRepository(AppDbContext context) : IStudentLicenseHistoryRepository
{
    public async Task<IReadOnlyList<StudentLicenseHistory>> GetAllAsync(int? studentLicenseId, int? studentId, int? licenseId, string? academicYear, int? semesterNumber, string? semesterLabel, string? studentName, string? licenseName, CancellationToken cancellationToken)
    {
        IQueryable<StudentLicenseHistory> query = context.StudentLicenseHistories
            .AsNoTracking()
            .Include(history => history.StudentLicense)
                .ThenInclude(studentLicense => studentLicense.Student)
            .Include(history => history.StudentLicense)
                .ThenInclude(studentLicense => studentLicense.License);

        if (studentLicenseId.HasValue)
            query = query.Where(history => history.StudentLicenseId == studentLicenseId.Value);

        if (studentId.HasValue)
            query = query.Where(history => history.StudentLicense.StudentId == studentId.Value);

        if (licenseId.HasValue)
            query = query.Where(history => history.StudentLicense.LicenseId == licenseId.Value);

        if (!string.IsNullOrWhiteSpace(academicYear))
            query = query.Where(history => history.AcademicYear != null && EF.Functions.ILike(history.AcademicYear, $"%{academicYear.Trim()}%"));

        if (semesterNumber.HasValue)
            query = query.Where(history => history.SemesterNumber == semesterNumber.Value);

        if (!string.IsNullOrWhiteSpace(semesterLabel))
            query = query.Where(history => EF.Functions.ILike(history.SemesterLabel, $"%{semesterLabel.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(studentName))
        {
            string searchPattern = $"%{studentName.Trim()}%";

            query = query.Where(history =>
                EF.Functions.ILike(history.StudentLicense.Student.LastName, searchPattern) ||
                EF.Functions.ILike(history.StudentLicense.Student.FirstName, searchPattern) ||
                (history.StudentLicense.Student.Patronymic != null && EF.Functions.ILike(history.StudentLicense.Student.Patronymic, searchPattern)));
        }

        if (!string.IsNullOrWhiteSpace(licenseName))
            query = query.Where(history => EF.Functions.ILike(history.StudentLicense.License.Name, $"%{licenseName.Trim()}%"));

        return await query.OrderBy(history => history.AcademicYear)
            .ThenBy(history => history.SemesterNumber)
            .ThenBy(history => history.SemesterLabel)
            .ToListAsync(cancellationToken);
    }

    public async Task<StudentLicenseHistory?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.StudentLicenseHistories
            .Include(history => history.StudentLicense)
                .ThenInclude(studentLicense => studentLicense.Student)
            .Include(history => history.StudentLicense)
                .ThenInclude(studentLicense => studentLicense.License)
            .FirstOrDefaultAsync(history => history.Id == id, cancellationToken);

    public async Task AddAsync(StudentLicenseHistory studentLicenseHistory, CancellationToken cancellationToken)
        => await context.StudentLicenseHistories.AddAsync(studentLicenseHistory, cancellationToken);

    public void Update(StudentLicenseHistory studentLicenseHistory)
        => context.StudentLicenseHistories.Update(studentLicenseHistory);

    public void Delete(StudentLicenseHistory studentLicenseHistory)
        => context.StudentLicenseHistories.Remove(studentLicenseHistory);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
