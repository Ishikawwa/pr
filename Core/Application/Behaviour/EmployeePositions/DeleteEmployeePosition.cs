using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.EmployeePositions;

public sealed record DeleteEmployeePositionCommand(int Id) : IRequest<bool>;

public sealed class DeleteEmployeePositionHandler(IEmployeePositionRepository repository) : IRequestHandler<DeleteEmployeePositionCommand, bool>
{
    public async Task<bool> Handle(DeleteEmployeePositionCommand request, CancellationToken cancellationToken)
    {
        EmployeePosition? employeePosition = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (employeePosition is null)
            return false;

        repository.Delete(employeePosition);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}