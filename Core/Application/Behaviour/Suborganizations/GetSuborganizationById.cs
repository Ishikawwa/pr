
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Suborganizations;

public sealed record GetSuborganizationByIdQuery(int Id) : IRequest<Suborganization?>;

public sealed class GetSuborganizationByIdHandler(ISuborganizationRepository repository) : IRequestHandler<GetSuborganizationByIdQuery, Suborganization?>
{
    public async Task<Suborganization?> Handle(GetSuborganizationByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}
