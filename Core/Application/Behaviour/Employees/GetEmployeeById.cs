using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Employees;

public sealed record GetEmployeeByIdQuery(int Id) : IRequest<Employee?>;

public sealed class GetEmployeeByIdHandler(IEmployeeRepository repository) : IRequestHandler<GetEmployeeByIdQuery, Employee?>
{
    public async Task<Employee?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}
