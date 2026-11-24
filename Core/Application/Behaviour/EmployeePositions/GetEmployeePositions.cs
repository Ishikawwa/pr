using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.EmployeePositions;

public sealed record GetEmployeePositionsQuery(int? EmployeeId, int? PositionId, string? EmployeeName, string? PositionTitle) : IRequest<IReadOnlyList<EmployeePosition>>;

public sealed class GetEmployeePositionsHandler(IEmployeePositionRepository repository) : IRequestHandler<GetEmployeePositionsQuery, IReadOnlyList<EmployeePosition>>
{
    public async Task<IReadOnlyList<EmployeePosition>> Handle(GetEmployeePositionsQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.EmployeeId, request.PositionId, request.EmployeeName, request.PositionTitle, cancellationToken);
}