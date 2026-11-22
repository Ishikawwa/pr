using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Positions;

public sealed record GetPositionByIdQuery(int Id) : IRequest<Position?>;

public sealed class GetPositionByIdHandler(IPositionRepository repository) : IRequestHandler<GetPositionByIdQuery, Position?>
{
    public async Task<Position?> Handle(GetPositionByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}
