using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.EmployeePositions;

public sealed record CreateEmployeePositionCommand(int EmployeeId, int PositionId) : IRequest<int>;

public sealed class CreateEmployeePositionHandler(IEmployeePositionRepository repository) : IRequestHandler<CreateEmployeePositionCommand, int>
{
    public async Task<int> Handle(CreateEmployeePositionCommand request, CancellationToken cancellationToken)
    {
        EmployeePosition employeePosition = new()
        {
            EmployeeId = request.EmployeeId,
            PositionId = request.PositionId
        };

        await repository.AddAsync(employeePosition, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return employeePosition.Id;
    }
}