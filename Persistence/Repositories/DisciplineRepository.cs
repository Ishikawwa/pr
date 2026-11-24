using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class DisciplineRepository(AppDbContext context) : IDisciplineRepository
{
    public async Task<IReadOnlyList<Discipline>> GetAllAsync(string? code, string? name, int? suborganizationId, CancellationToken cancellationToken)
    {
        IQueryable<Discipline> query = context.Disciplines.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(code))
            query = query.Where(discipline => EF.Functions.ILike(discipline.Code, $"%{code.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(discipline => EF.Functions.ILike(discipline.Name, $"%{name.Trim()}%"));

        if (suborganizationId.HasValue)
            query = query.Where(discipline => discipline.SuborganizationId == suborganizationId.Value);

        return await query.OrderBy(discipline => discipline.Code).ThenBy(discipline => discipline.Name).ToListAsync(cancellationToken);
    }

    public async Task<Discipline?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.Disciplines.FirstOrDefaultAsync(discipline => discipline.Id == id, cancellationToken);

    public async Task AddAsync(Discipline discipline, CancellationToken cancellationToken)
        => await context.Disciplines.AddAsync(discipline, cancellationToken);

    public void Update(Discipline discipline)
        => context.Disciplines.Update(discipline);

    public void Delete(Discipline discipline)
        => context.Disciplines.Remove(discipline);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
