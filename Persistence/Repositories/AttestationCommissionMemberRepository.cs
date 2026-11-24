using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class AttestationCommissionMemberRepository(AppDbContext context) : IAttestationCommissionMemberRepository
{
    public async Task<IReadOnlyList<AttestationCommissionMember>> GetAllAsync(int? commissionId, string? fullName, string? position, string? role, int? sortOrder, CancellationToken cancellationToken)
    {
        IQueryable<AttestationCommissionMember> query = context.AttestationCommissionMembers.AsNoTracking();

        if (commissionId.HasValue)
            query = query.Where(member => member.CommissionId == commissionId.Value);

        if (!string.IsNullOrWhiteSpace(fullName))
            query = query.Where(member => EF.Functions.ILike(member.FullName, $"%{fullName.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(position))
            query = query.Where(member => member.Position != null && EF.Functions.ILike(member.Position, $"%{position.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(role))
            query = query.Where(member => EF.Functions.ILike(member.Role, $"%{role.Trim()}%"));

        if (sortOrder.HasValue)
            query = query.Where(member => member.SortOrder == sortOrder.Value);

        return await query.OrderBy(member => member.CommissionId)
            .ThenBy(member => member.SortOrder)
            .ThenBy(member => member.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<AttestationCommissionMember?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.AttestationCommissionMembers.FirstOrDefaultAsync(member => member.Id == id, cancellationToken);

    public async Task AddAsync(AttestationCommissionMember member, CancellationToken cancellationToken)
        => await context.AttestationCommissionMembers.AddAsync(member, cancellationToken);

    public void Update(AttestationCommissionMember member)
        => context.AttestationCommissionMembers.Update(member);

    public void Delete(AttestationCommissionMember member)
        => context.AttestationCommissionMembers.Remove(member);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}