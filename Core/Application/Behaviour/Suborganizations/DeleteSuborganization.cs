
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Suborganizations;

public sealed record DeleteSuborganizationCommand(int Id) : IRequest<bool>;

public sealed class DeleteSuborganizationHandler(ISuborganizationRepository repository) : IRequestHandler<DeleteSuborganizationCommand, bool>
{
    public async Task<bool> Handle(DeleteSuborganizationCommand request, CancellationToken cancellationToken)
    {
        Suborganization? suborganization = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (suborganization is null)
            return false;

        repository.Delete(suborganization);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
