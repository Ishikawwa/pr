using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Disciplines;

public sealed record GetDisciplineByIdQuery(int Id) : IRequest<Discipline?>;

public sealed class GetDisciplineByIdHandler(IDisciplineRepository repository) : IRequestHandler<GetDisciplineByIdQuery, Discipline?>
{
    public async Task<Discipline?> Handle(GetDisciplineByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}
