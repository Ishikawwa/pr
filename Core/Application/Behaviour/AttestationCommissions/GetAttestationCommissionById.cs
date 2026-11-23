using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.AttestationCommissions;

public sealed record GetAttestationCommissionByIdQuery(int Id) : IRequest<AttestationCommission?>;

public sealed class GetAttestationCommissionByIdHandler(IAttestationCommissionRepository repository) : IRequestHandler<GetAttestationCommissionByIdQuery, AttestationCommission?>
{
    public async Task<AttestationCommission?> Handle(GetAttestationCommissionByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}