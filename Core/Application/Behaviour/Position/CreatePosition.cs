using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Positions;

public sealed record CreatePositionCommand(string Title) : IRequest<int>;

public sealed class CreatePositionHandler(IPositionRepository repository) : IRequestHandler<CreatePositionCommand, int>
{
    public async Task<int> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        Position position = new()
        {
            Title = request.Title
        };

        await repository.AddAsync(position, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return position.Id;
    }
}
