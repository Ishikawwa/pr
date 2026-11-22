using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Licenses;

public sealed record CreateLicenseCommand(string Name) : IRequest<int>;

public sealed class CreateLicenseHandler(ILicenseRepository repository) : IRequestHandler<CreateLicenseCommand, int>
{
    public async Task<int> Handle(CreateLicenseCommand request, CancellationToken cancellationToken)
    {
        License license = new()
        {
            Name = request.Name
        };

        await repository.AddAsync(license, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return license.Id;
    }
}
