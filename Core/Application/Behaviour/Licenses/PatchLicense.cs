
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Licenses;

public sealed record PatchLicenseCommand(int Id, string? Name) : IRequest<bool>;

public sealed class PatchLicenseHandler(ILicenseRepository repository) : IRequestHandler<PatchLicenseCommand, bool>
{
    public async Task<bool> Handle(PatchLicenseCommand request, CancellationToken cancellationToken)
    {
        License? license = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (license is null)
            return false;

        if (request.Name is not null)
            license.Name = request.Name;

        repository.Update(license);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
