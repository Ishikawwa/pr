using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IStudentLicenseHistoryRepository
{
    Task<IReadOnlyList<StudentLicenseHistory>> GetAllAsync(int? studentLicenseId, int? studentId, int? licenseId, string? academicYear, int? semesterNumber, string? semesterLabel, string? studentName, string? licenseName, CancellationToken cancellationToken);
    Task<StudentLicenseHistory?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(StudentLicenseHistory studentLicenseHistory, CancellationToken cancellationToken);
    void Update(StudentLicenseHistory studentLicenseHistory);
    void Delete(StudentLicenseHistory studentLicenseHistory);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
