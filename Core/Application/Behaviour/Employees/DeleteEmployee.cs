using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Employees;

public sealed record DeleteEmployeeCommand(int Id) : IRequest<bool>;

public sealed class DeleteEmployeeHandler(IEmployeeRepository repository) : IRequestHandler<DeleteEmployeeCommand, bool>
{
    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        Employee? employee = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (employee is null)
            return false;

        repository.Delete(employee);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
