using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.AttestationCommissionMembers;

public sealed record PatchAttestationCommissionMemberCommand(int Id, int? CommissionId, string? FullName, string? Position, string? Role, int? SortOrder) : IRequest<bool>;

public sealed class PatchAttestationCommissionMemberHandler(IAttestationCommissionMemberRepository repository) : IRequestHandler<PatchAttestationCommissionMemberCommand, bool>
{
    public async Task<bool> Handle(PatchAttestationCommissionMemberCommand request, CancellationToken cancellationToken)
    {
        AttestationCommissionMember? member = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (member is null)
            return false;

        if (request.CommissionId.HasValue)
            member.CommissionId = request.CommissionId.Value;

        if (request.FullName is not null)
            member.FullName = request.FullName;

        if (request.Position is not null)
            member.Position = request.Position;

        if (request.Role is not null)
            member.Role = request.Role;

        if (request.SortOrder.HasValue)
            member.SortOrder = request.SortOrder.Value;

        repository.Update(member);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}