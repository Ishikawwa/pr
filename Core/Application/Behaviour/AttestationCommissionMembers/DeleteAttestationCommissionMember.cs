using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.AttestationCommissionMembers;

public sealed record DeleteAttestationCommissionMemberCommand(int Id) : IRequest<bool>;

public sealed class DeleteAttestationCommissionMemberHandler(IAttestationCommissionMemberRepository repository) : IRequestHandler<DeleteAttestationCommissionMemberCommand, bool>
{
    public async Task<bool> Handle(DeleteAttestationCommissionMemberCommand request, CancellationToken cancellationToken)
    {
        AttestationCommissionMember? member = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (member is null)
            return false;

        repository.Delete(member);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}