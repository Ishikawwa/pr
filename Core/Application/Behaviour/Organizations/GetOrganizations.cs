using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Organizations;

public sealed record GetOrganizationsQuery : IRequest<IReadOnlyList<Organization>>;
public sealed class GetOrganizationsHandler(IOrganizationRepository repository) : IRequestHandler<GetOrganizationsQuery, IReadOnlyList<Organization>>
{
    public async Task<IReadOnlyList<Organization>> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(cancellationToken);
}