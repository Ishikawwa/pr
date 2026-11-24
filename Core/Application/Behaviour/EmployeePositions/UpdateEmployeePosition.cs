using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.EmployeePositions;

public sealed record UpdateEmployeePositionCommand(int Id, int EmployeeId, int PositionId) : IRequest<bool>;

public sealed class UpdateEmployeePositionHandler(IEmployeePositionRepository repository) : IRequestHandler<UpdateEmployeePositionCommand, bool>
{
    public async Task<bool> Handle(UpdateEmployeePositionCommand request, CancellationToken cancellationToken)
    {
        EmployeePosition? employeePosition = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (employeePosition is null)
            return false;

        employeePosition.EmployeeId = request.EmployeeId;
        employeePosition.PositionId = request.PositionId;

        repository.Update(employeePosition);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}