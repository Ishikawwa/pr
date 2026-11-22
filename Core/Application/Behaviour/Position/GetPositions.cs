using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Positions;

public sealed record GetPositionsQuery(string? Title) : IRequest<IReadOnlyList<Position>>;

public sealed class GetPositionsHandler(IPositionRepository repository) : IRequestHandler<GetPositionsQuery, IReadOnlyList<Position>>
{
    public async Task<IReadOnlyList<Position>> Handle(GetPositionsQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.Title, cancellationToken);
}
