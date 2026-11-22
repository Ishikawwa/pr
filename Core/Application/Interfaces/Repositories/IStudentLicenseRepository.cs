using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IStudentLicenseRepository
{
    Task<IReadOnlyList<StudentLicense>> GetAllAsync(int? studentId, int? licenseId, string? studentName, string? licenseName, CancellationToken cancellationToken);
    Task<StudentLicense?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(StudentLicense studentLicense, CancellationToken cancellationToken);
    void Update(StudentLicense studentLicense);
    void Delete(StudentLicense studentLicense);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
