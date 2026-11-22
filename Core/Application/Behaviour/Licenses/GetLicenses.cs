using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Licenses;

public sealed record GetLicensesQuery(string? Name) : IRequest<IReadOnlyList<License>>;

public sealed class GetLicensesHandler(ILicenseRepository repository) : IRequestHandler<GetLicensesQuery, IReadOnlyList<License>>
{
    public async Task<IReadOnlyList<License>> Handle(GetLicensesQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.Name, cancellationToken);
}
