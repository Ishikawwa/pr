
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Suborganizations;

public sealed record UpdateSuborganizationCommand(int Id, string Name, int OrganizationId) : IRequest<bool>;

public sealed class UpdateSuborganizationHandler(ISuborganizationRepository repository) : IRequestHandler<UpdateSuborganizationCommand, bool>
{
    public async Task<bool> Handle(UpdateSuborganizationCommand request, CancellationToken cancellationToken)
    {
        Suborganization? suborganization = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (suborganization is null)
            return false;

        suborganization.Name = request.Name;
        suborganization.OrganizationId = request.OrganizationId;

        repository.Update(suborganization);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
