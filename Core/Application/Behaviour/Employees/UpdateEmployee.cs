using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Employees;

public sealed record UpdateEmployeeCommand(int Id, string FullName) : IRequest<bool>;

public sealed class UpdateEmployeeHandler(IEmployeeRepository repository) : IRequestHandler<UpdateEmployeeCommand, bool>
{
    public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        Employee? employee = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (employee is null)
            return false;

        employee.FullName = request.FullName;

        repository.Update(employee);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
