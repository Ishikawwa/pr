using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    public async Task<IReadOnlyList<Employee>> GetAllAsync(string? fullName, CancellationToken cancellationToken)
    {
        IQueryable<Employee> query = context.Employees.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(fullName))
            query = query.Where(employee => EF.Functions.ILike(employee.FullName, $"%{fullName.Trim()}%"));

        return await query.OrderBy(employee => employee.FullName).ToListAsync(cancellationToken);
    }

    public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.Employees.FirstOrDefaultAsync(employee => employee.Id == id, cancellationToken);

    public async Task AddAsync(Employee employee, CancellationToken cancellationToken)
        => await context.Employees.AddAsync(employee, cancellationToken);

    public void Update(Employee employee)
        => context.Employees.Update(employee);

    public void Delete(Employee employee)
        => context.Employees.Remove(employee);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
