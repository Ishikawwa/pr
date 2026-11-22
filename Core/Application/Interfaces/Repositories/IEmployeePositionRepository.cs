using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IEmployeePositionRepository
{
    Task<IReadOnlyList<EmployeePosition>> GetAllAsync(int? employeeId, int? positionId, string? employeeName, string? positionTitle, CancellationToken cancellationToken);
    Task<EmployeePosition?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(EmployeePosition employeePosition, CancellationToken cancellationToken);
    void Update(EmployeePosition employeePosition);
    void Delete(EmployeePosition employeePosition);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}       