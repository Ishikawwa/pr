using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.AttestationCommissions;

public sealed record DeleteAttestationCommissionCommand(int Id) : IRequest<bool>;

public sealed class DeleteAttestationCommissionHandler(IAttestationCommissionRepository repository) : IRequestHandler<DeleteAttestationCommissionCommand, bool>
{
    public async Task<bool> Handle(DeleteAttestationCommissionCommand request, CancellationToken cancellationToken)
    {
        AttestationCommission? commission = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (commission is null)
            return false;

        repository.Delete(commission);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}