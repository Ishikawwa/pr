using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAttestationCommissionRepository
{
    Task<IReadOnlyList<AttestationCommission>> GetAllAsync(int? disciplineId, int? studyGroupId, int? studyProgramId, string? academicYear, string? semesterLabel, int? semesterNumber, string? disciplineName, string? studyGroupName, CancellationToken cancellationToken);
    Task<AttestationCommission?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(AttestationCommission commission, CancellationToken cancellationToken);
    void Update(AttestationCommission commission);
    void Delete(AttestationCommission commission);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}