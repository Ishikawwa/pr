using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.FgosDisciplines;

public sealed record GetFgosDisciplineByIdQuery(int Id) : IRequest<FgosDiscipline?>;

public sealed class GetFgosDisciplineByIdHandler(IFgosDisciplineRepository repository) : IRequestHandler<GetFgosDisciplineByIdQuery, FgosDiscipline?>
{
    public async Task<FgosDiscipline?> Handle(GetFgosDisciplineByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}
