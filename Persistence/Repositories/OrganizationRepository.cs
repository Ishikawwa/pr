using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class OrganizationRepository(AppDbContext context) : IOrganizationRepository
{
    public async Task<IReadOnlyList<Organization>> GetAllAsync(string? name, CancellationToken cancellationToken)
    {
        IQueryable<Organization> query = context.Organizations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
        {
            string pattern = $"%{name.Trim()}%";

            query = query.Where(organization 
                => EF.Functions.ILike(organization.Name, pattern));
        }

        return await query.OrderBy(organization => organization.Name).ToListAsync(cancellationToken);
    }

    public async Task<Organization?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.Organizations.FirstOrDefaultAsync(organization => organization.Id == id, cancellationToken);

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId, CancellationToken cancellationToken)
        => await context.Organizations.AnyAsync(organization => organization.Name == name && (!excludeId.HasValue || organization.Id != excludeId.Value), cancellationToken);

    public async Task AddAsync(Organization organization, CancellationToken cancellationToken)
        => await context.Organizations.AddAsync(organization, cancellationToken);

    public void Update(Organization organization)
        => context.Organizations.Update(organization);

    public void Delete(Organization organization)
        => context.Organizations.Remove(organization);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}