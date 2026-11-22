
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Behaviour.Positions;

public sealed record DeletePositionCommand(int Id) : IRequest<bool>;

public sealed class DeletePositionHandler(IPositionRepository repository) : IRequestHandler<DeletePositionCommand, bool>
{
    public async Task<bool> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Position? position = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (position is null)
            return false;

        repository.Delete(position);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
