using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class FgosDisciplineMappingRepository(AppDbContext context) : IFgosDisciplineMappingRepository
{
    public async Task<IReadOnlyList<FgosDisciplineMapping>> GetAllAsync(int? disciplineId, int? fgosDisciplineId, string? disciplineCode, string? disciplineName, string? fgosDisciplineCode, string? fgosDisciplineName, CancellationToken cancellationToken)
    {
        IQueryable<FgosDisciplineMapping> query = context.FgosDisciplineMappings
            .AsNoTracking()
            .Include(mapping => mapping.Discipline)
            .Include(mapping => mapping.FgosDiscipline);

        if (disciplineId.HasValue)
            query = query.Where(mapping => mapping.DisciplineId == disciplineId.Value);

        if (fgosDisciplineId.HasValue)
            query = query.Where(mapping => mapping.FgosDisciplineId == fgosDisciplineId.Value);

        if (!string.IsNullOrWhiteSpace(disciplineCode))
            query = query.Where(mapping => EF.Functions.ILike(mapping.Discipline.Code, $"%{disciplineCode.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(disciplineName))
            query = query.Where(mapping => EF.Functions.ILike(mapping.Discipline.Name, $"%{disciplineName.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(fgosDisciplineCode))
            query = query.Where(mapping => EF.Functions.ILike(mapping.FgosDiscipline.Code, $"%{fgosDisciplineCode.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(fgosDisciplineName))
            query = query.Where(mapping => EF.Functions.ILike(mapping.FgosDiscipline.Name, $"%{fgosDisciplineName.Trim()}%"));

        return await query.OrderBy(mapping => mapping.Discipline.Name)
            .ThenBy(mapping => mapping.FgosDiscipline.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<FgosDisciplineMapping?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.FgosDisciplineMappings
            .Include(mapping => mapping.Discipline)
            .Include(mapping => mapping.FgosDiscipline)
            .FirstOrDefaultAsync(mapping => mapping.Id == id, cancellationToken);

    public async Task AddAsync(FgosDisciplineMapping mapping, CancellationToken cancellationToken)
        => await context.FgosDisciplineMappings.AddAsync(mapping, cancellationToken);

    public void Update(FgosDisciplineMapping mapping)
        => context.FgosDisciplineMappings.Update(mapping);

    public void Delete(FgosDisciplineMapping mapping)
        => context.FgosDisciplineMappings.Remove(mapping);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
