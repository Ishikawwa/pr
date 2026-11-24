using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.EmployeePositions;

public sealed record GetEmployeePositionByIdQuery(int Id) : IRequest<EmployeePosition?>;

public sealed class GetEmployeePositionByIdHandler(IEmployeePositionRepository repository) : IRequestHandler<GetEmployeePositionByIdQuery, EmployeePosition?>
{
    public async Task<EmployeePosition?> Handle(GetEmployeePositionByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}