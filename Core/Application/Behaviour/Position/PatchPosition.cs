using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Positions;

public sealed record PatchPositionCommand(int Id, string? Title) : IRequest<bool>;

public sealed class PatchPositionHandler(IPositionRepository repository) : IRequestHandler<PatchPositionCommand, bool>
{
    public async Task<bool> Handle(PatchPositionCommand request, CancellationToken cancellationToken)
    {
        Position? position = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (position is null)
            return false;

        if (request.Title is not null)
            position.Title = request.Title;

        repository.Update(position);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
