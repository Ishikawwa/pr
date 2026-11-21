
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Suborganizations;

public sealed record PatchSuborganizationCommand(int Id, string? Name, int? OrganizationId) : IRequest<bool>;

public sealed class PatchSuborganizationHandler(ISuborganizationRepository repository) : IRequestHandler<PatchSuborganizationCommand, bool>
{
    public async Task<bool> Handle(PatchSuborganizationCommand request, CancellationToken cancellationToken)
    {
        Suborganization? suborganization = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (suborganization is null)
            return false;

        if (request.Name is not null)
            suborganization.Name = request.Name;

        if (request.OrganizationId.HasValue)
            suborganization.OrganizationId = request.OrganizationId.Value;

        repository.Update(suborganization);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
