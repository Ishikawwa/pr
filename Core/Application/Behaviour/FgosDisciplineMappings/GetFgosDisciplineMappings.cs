using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.FgosDisciplineMappings;

public sealed record GetFgosDisciplineMappingsQuery(int? DisciplineId, int? FgosDisciplineId, string? DisciplineCode, string? DisciplineName, string? FgosDisciplineCode, string? FgosDisciplineName) : IRequest<IReadOnlyList<FgosDisciplineMapping>>;

public sealed class GetFgosDisciplineMappingsHandler(IFgosDisciplineMappingRepository repository) : IRequestHandler<GetFgosDisciplineMappingsQuery, IReadOnlyList<FgosDisciplineMapping>>
{
    public async Task<IReadOnlyList<FgosDisciplineMapping>> Handle(GetFgosDisciplineMappingsQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.DisciplineId, request.FgosDisciplineId, request.DisciplineCode, request.DisciplineName, request.FgosDisciplineCode, request.FgosDisciplineName, cancellationToken);
}
