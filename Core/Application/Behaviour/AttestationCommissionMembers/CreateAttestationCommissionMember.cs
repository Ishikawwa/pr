using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.AttestationCommissionMembers;

public sealed record CreateAttestationCommissionMemberCommand(int CommissionId, string FullName, string? Position, string Role, int SortOrder) : IRequest<int>;

public sealed class CreateAttestationCommissionMemberHandler(IAttestationCommissionMemberRepository repository) : IRequestHandler<CreateAttestationCommissionMemberCommand, int>
{
    public async Task<int> Handle(CreateAttestationCommissionMemberCommand request, CancellationToken cancellationToken)
    {
        AttestationCommissionMember member = new()
        {
            CommissionId = request.CommissionId,
            FullName = request.FullName,
            Position = request.Position,
            Role = request.Role,
            SortOrder = request.SortOrder
        };

        await repository.AddAsync(member, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return member.Id;
    }
}