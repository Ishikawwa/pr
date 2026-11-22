using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Licenses;

public sealed record UpdateLicenseCommand(int Id, string Name) : IRequest<bool>;

public sealed class UpdateLicenseHandler(ILicenseRepository repository) : IRequestHandler<UpdateLicenseCommand, bool>
{
    public async Task<bool> Handle(UpdateLicenseCommand request, CancellationToken cancellationToken)
    {
        License? license = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (license is null)
            return false;

        license.Name = request.Name;

        repository.Update(license);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
