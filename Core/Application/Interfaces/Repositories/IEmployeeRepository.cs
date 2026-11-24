using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task<IReadOnlyList<Employee>> GetAllAsync(string? fullName, CancellationToken cancellationToken);
    Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Employee employee, CancellationToken cancellationToken);
    void Update(Employee employee);
    void Delete(Employee employee);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
