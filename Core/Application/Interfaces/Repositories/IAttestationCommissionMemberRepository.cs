using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAttestationCommissionMemberRepository
{
    Task<IReadOnlyList<AttestationCommissionMember>> GetAllAsync(int? commissionId, string? fullName, string? position, string? role, int? sortOrder, CancellationToken cancellationToken);
    Task<AttestationCommissionMember?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(AttestationCommissionMember member, CancellationToken cancellationToken);
    void Update(AttestationCommissionMember member);
    void Delete(AttestationCommissionMember member);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}