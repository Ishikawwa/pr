
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Suborganizations;

public sealed record GetSuborganizationsQuery(string? Name, int? OrganizationId) : IRequest<IReadOnlyList<Suborganization>>;

public sealed class GetSuborganizationsHandler(ISuborganizationRepository repository) : IRequestHandler<GetSuborganizationsQuery, IReadOnlyList<Suborganization>>
{
    public async Task<IReadOnlyList<Suborganization>> Handle(GetSuborganizationsQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.Name, request.OrganizationId, cancellationToken);
}
