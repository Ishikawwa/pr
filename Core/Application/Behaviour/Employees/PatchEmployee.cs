using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Employees;

public sealed record PatchEmployeeCommand(int Id, string? FullName) : IRequest<bool>;

public sealed class PatchEmployeeHandler(IEmployeeRepository repository) : IRequestHandler<PatchEmployeeCommand, bool>
{
    public async Task<bool> Handle(PatchEmployeeCommand request, CancellationToken cancellationToken)
    {
        Employee? employee = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (employee is null)
            return false;

        if (request.FullName is not null)
            employee.FullName = request.FullName;

        repository.Update(employee);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
