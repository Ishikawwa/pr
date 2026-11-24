
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Suborganizations;

public sealed record CreateSuborganizationCommand(string Name, int OrganizationId) : IRequest<int>;

public sealed class CreateSuborganizationHandler(ISuborganizationRepository repository) : IRequestHandler<CreateSuborganizationCommand, int>
{
    public async Task<int> Handle(CreateSuborganizationCommand request, CancellationToken cancellationToken)
    {
        Suborganization suborganization = new()
        {
            Name = request.Name,
            OrganizationId = request.OrganizationId
        };

        await repository.AddAsync(suborganization, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return suborganization.Id;
    }
}
