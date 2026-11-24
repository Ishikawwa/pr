using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Licenses;

public sealed record DeleteLicenseCommand(int Id) : IRequest<bool>;

public sealed class DeleteLicenseHandler(ILicenseRepository repository) : IRequestHandler<DeleteLicenseCommand, bool>
{
    public async Task<bool> Handle(DeleteLicenseCommand request, CancellationToken cancellationToken)
    {
        License? license = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (license is null)
            return false;

        repository.Delete(license);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
