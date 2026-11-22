using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Disciplines;

public sealed record GetDisciplinesQuery(string? Code, string? Name, int? SuborganizationId) : IRequest<IReadOnlyList<Discipline>>;

public sealed class GetDisciplinesHandler(IDisciplineRepository repository) : IRequestHandler<GetDisciplinesQuery, IReadOnlyList<Discipline>>
{
    public async Task<IReadOnlyList<Discipline>> Handle(GetDisciplinesQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.Code, request.Name, request.SuborganizationId, cancellationToken);
}
