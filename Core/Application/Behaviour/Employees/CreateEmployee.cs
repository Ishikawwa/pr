using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Employees;

public sealed record CreateEmployeeCommand(string FullName) : IRequest<int>;

public sealed class CreateEmployeeHandler(IEmployeeRepository repository) : IRequestHandler<CreateEmployeeCommand, int>
{
    public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        Employee employee = new()
        {
            FullName = request.FullName
        };

        await repository.AddAsync(employee, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return employee.Id;
    }
}
