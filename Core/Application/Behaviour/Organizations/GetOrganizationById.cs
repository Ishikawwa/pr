using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Organizations;

public sealed record GetOrganizationByIdQuery(int Id) : IRequest<Organization?>;

public sealed class GetOrganizationByIdHandler(IOrganizationRepository repository) : IRequestHandler<GetOrganizationByIdQuery, Organization?>
{
    public async Task<Organization?> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}