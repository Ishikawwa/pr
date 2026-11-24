using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.FgosDisciplineMappings;

public sealed record GetFgosDisciplineMappingByIdQuery(int Id) : IRequest<FgosDisciplineMapping?>;

public sealed class GetFgosDisciplineMappingByIdHandler(IFgosDisciplineMappingRepository repository) : IRequestHandler<GetFgosDisciplineMappingByIdQuery, FgosDisciplineMapping?>
{
    public async Task<FgosDisciplineMapping?> Handle(GetFgosDisciplineMappingByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}
