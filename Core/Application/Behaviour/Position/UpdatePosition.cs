using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Behaviour.Positions;

public sealed record UpdatePositionCommand(int Id, string Title) : IRequest<bool>;

public sealed class UpdatePositionHandler(IPositionRepository repository) : IRequestHandler<UpdatePositionCommand, bool>
{
    public async Task<bool> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Position? position = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (position is null)
            return false;

        position.Title = request.Title;

        repository.Update(position);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
