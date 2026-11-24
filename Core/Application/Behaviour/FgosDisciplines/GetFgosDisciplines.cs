using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.FgosDisciplines;

public sealed record GetFgosDisciplinesQuery(string? Name, string? Code, int? StudyProgramId) : IRequest<IReadOnlyList<FgosDiscipline>>;

public sealed class GetFgosDisciplinesHandler(IFgosDisciplineRepository repository) : IRequestHandler<GetFgosDisciplinesQuery, IReadOnlyList<FgosDiscipline>>
{
    public async Task<IReadOnlyList<FgosDiscipline>> Handle(GetFgosDisciplinesQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.Name, request.Code, request.StudyProgramId, cancellationToken);
}
