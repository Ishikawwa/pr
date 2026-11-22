using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.EmployeePositions;

public sealed record PatchEmployeePositionCommand(int Id, int? EmployeeId, int? PositionId) : IRequest<bool>;

public sealed class PatchEmployeePositionHandler(IEmployeePositionRepository repository) : IRequestHandler<PatchEmployeePositionCommand, bool>
{
    public async Task<bool> Handle(PatchEmployeePositionCommand request, CancellationToken cancellationToken)
    {
        EmployeePosition? employeePosition = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (employeePosition is null)
            return false;

        if (request.EmployeeId.HasValue)
            employeePosition.EmployeeId = request.EmployeeId.Value;

        if (request.PositionId.HasValue)
            employeePosition.PositionId = request.PositionId.Value;

        repository.Update(employeePosition);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}