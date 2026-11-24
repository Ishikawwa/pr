using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.AttestationCommissionMembers;

public sealed record UpdateAttestationCommissionMemberCommand(int Id, int CommissionId, string FullName, string? Position, string Role, int SortOrder) : IRequest<bool>;

public sealed class UpdateAttestationCommissionMemberHandler(IAttestationCommissionMemberRepository repository) : IRequestHandler<UpdateAttestationCommissionMemberCommand, bool>
{
    public async Task<bool> Handle(UpdateAttestationCommissionMemberCommand request, CancellationToken cancellationToken)
    {
        AttestationCommissionMember? member = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (member is null)
            return false;

        member.CommissionId = request.CommissionId;
        member.FullName = request.FullName;
        member.Position = request.Position;
        member.Role = request.Role;
        member.SortOrder = request.SortOrder;

        repository.Update(member);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}