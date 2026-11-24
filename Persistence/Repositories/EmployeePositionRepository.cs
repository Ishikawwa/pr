using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class EmployeePositionRepository(AppDbContext context) : IEmployeePositionRepository
{
    public async Task<IReadOnlyList<EmployeePosition>> GetAllAsync(int? employeeId, int? positionId, string? employeeName, string? positionTitle, CancellationToken cancellationToken)
    {
        IQueryable<EmployeePosition> query = context.EmployeePositions
            .AsNoTracking()
            .Include(employeePosition => employeePosition.Employee)
            .Include(employeePosition => employeePosition.Position);

        if (employeeId.HasValue)
            query = query.Where(employeePosition => employeePosition.EmployeeId == employeeId.Value);

        if (positionId.HasValue)
            query = query.Where(employeePosition => employeePosition.PositionId == positionId.Value);

        if (!string.IsNullOrWhiteSpace(employeeName))
            query = query.Where(employeePosition => EF.Functions.ILike(employeePosition.Employee.FullName, $"%{employeeName.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(positionTitle))
            query = query.Where(employeePosition => EF.Functions.ILike(employeePosition.Position.Title, $"%{positionTitle.Trim()}%"));

        return await query.OrderBy(employeePosition => employeePosition.Employee.FullName)
            .ThenBy(employeePosition => employeePosition.Position.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<EmployeePosition?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.EmployeePositions
            .Include(employeePosition => employeePosition.Employee)
            .Include(employeePosition => employeePosition.Position)
            .FirstOrDefaultAsync(employeePosition => employeePosition.Id == id, cancellationToken);

    public async Task AddAsync(EmployeePosition employeePosition, CancellationToken cancellationToken)
        => await context.EmployeePositions.AddAsync(employeePosition, cancellationToken);

    public void Update(EmployeePosition employeePosition)
        => context.EmployeePositions.Update(employeePosition);

    public void Delete(EmployeePosition employeePosition)
        => context.EmployeePositions.Remove(employeePosition);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}