
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class SuborganizationRepository(AppDbContext context) : ISuborganizationRepository
{
    public async Task<IReadOnlyList<Suborganization>> GetAllAsync(string? name, int? organizationId, CancellationToken cancellationToken)
    {
        IQueryable<Suborganization> query = context.Suborganizations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(suborganization => EF.Functions.ILike(suborganization.Name, $"%{name}%"));

        if (organizationId.HasValue)
            query = query.Where(suborganization => suborganization.OrganizationId == organizationId.Value);

        return await query.OrderBy(suborganization => suborganization.Name).ToListAsync(cancellationToken);
    }

    public async Task<Suborganization?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.Suborganizations.FirstOrDefaultAsync(suborganization => suborganization.Id == id, cancellationToken);

    public async Task AddAsync(Suborganization suborganization, CancellationToken cancellationToken)
        => await context.Suborganizations.AddAsync(suborganization, cancellationToken);

    public void Update(Suborganization suborganization)
        => context.Suborganizations.Update(suborganization);

    public void Delete(Suborganization suborganization)
        => context.Suborganizations.Remove(suborganization);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
