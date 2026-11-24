using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class LicenseRepository(AppDbContext context) : ILicenseRepository
{
    public async Task<IReadOnlyList<License>> GetAllAsync(string? name, CancellationToken cancellationToken)
    {
        IQueryable<License> query = context.Licenses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(license => EF.Functions.ILike(license.Name, $"%{name.Trim()}%"));

        return await query.OrderBy(license => license.Name).ToListAsync(cancellationToken);
    }

    public async Task<License?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.Licenses.FirstOrDefaultAsync(license => license.Id == id, cancellationToken);

    public async Task AddAsync(License license, CancellationToken cancellationToken)
        => await context.Licenses.AddAsync(license, cancellationToken);

    public void Update(License license)
        => context.Licenses.Update(license);

    public void Delete(License license)
        => context.Licenses.Remove(license);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
