using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Licenses;

public sealed record GetLicenseByIdQuery(int Id) : IRequest<License?>;

public sealed class GetLicenseByIdHandler(ILicenseRepository repository) : IRequestHandler<GetLicenseByIdQuery, License?>
{
    public async Task<License?> Handle(GetLicenseByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}
