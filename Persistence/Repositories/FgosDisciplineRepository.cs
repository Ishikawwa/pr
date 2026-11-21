
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class FgosDisciplineRepository(AppDbContext context) : IFgosDisciplineRepository
{
    public async Task<IReadOnlyList<FgosDiscipline>> GetAllAsync(string? name, string? code, int? studyProgramId, CancellationToken cancellationToken)
    {
        IQueryable<FgosDiscipline> query = context.FgosDisciplines.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(fgosDiscipline => EF.Functions.ILike(fgosDiscipline.Name, $"%{name.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(code))
            query = query.Where(fgosDiscipline => EF.Functions.ILike(fgosDiscipline.Code, $"%{code.Trim()}%"));

        if (studyProgramId.HasValue)
            query = query.Where(fgosDiscipline => fgosDiscipline.StudyProgramId == studyProgramId.Value);

        return await query.OrderBy(fgosDiscipline => fgosDiscipline.Code).ToListAsync(cancellationToken);
    }

    public async Task<FgosDiscipline?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.FgosDisciplines.FirstOrDefaultAsync(fgosDiscipline => fgosDiscipline.Id == id, cancellationToken);

    public async Task AddAsync(FgosDiscipline fgosDiscipline, CancellationToken cancellationToken)
        => await context.FgosDisciplines.AddAsync(fgosDiscipline, cancellationToken);

    public void Update(FgosDiscipline fgosDiscipline)
        => context.FgosDisciplines.Update(fgosDiscipline);

    public void Delete(FgosDiscipline fgosDiscipline)
        => context.FgosDisciplines.Remove(fgosDiscipline);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
