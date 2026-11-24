using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Employees;

public sealed record GetEmployeesQuery(string? FullName) : IRequest<IReadOnlyList<Employee>>;

public sealed class GetEmployeesHandler(IEmployeeRepository repository) : IRequestHandler<GetEmployeesQuery, IReadOnlyList<Employee>>
{
    public async Task<IReadOnlyList<Employee>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.FullName, cancellationToken);
}
