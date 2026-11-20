using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Behaviour.Organizations;

public sealed record DeleteOrganizationCommand(int Id) : IRequest<bool>;

public sealed class DeleteOrganizationHandler(IOrganizationRepository repository) : IRequestHandler<DeleteOrganizationCommand, bool>
{
    public async Task<bool> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Organization? organization = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (organization is null)
            return false;

        repository.Delete(organization);

        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}