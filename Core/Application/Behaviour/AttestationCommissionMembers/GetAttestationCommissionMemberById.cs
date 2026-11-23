using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.AttestationCommissionMembers;

public sealed record GetAttestationCommissionMemberByIdQuery(int Id) : IRequest<AttestationCommissionMember?>;

public sealed class GetAttestationCommissionMemberByIdHandler(IAttestationCommissionMemberRepository repository) : IRequestHandler<GetAttestationCommissionMemberByIdQuery, AttestationCommissionMember?>
{
    public async Task<AttestationCommissionMember?> Handle(GetAttestationCommissionMemberByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}