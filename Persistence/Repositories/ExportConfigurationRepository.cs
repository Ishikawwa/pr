
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class ExportConfigurationRepository(AppDbContext context) : IExportConfigurationRepository
{
    public async Task<IReadOnlyList<ExportConfiguration>> GetAllAsync(string? search, string? templateKey, int? studyGroupId, bool? isActive, CancellationToken cancellationToken)
    {
        IQueryable<ExportConfiguration> query = context.ExportConfigurations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(configuration => EF.Functions.ILike(configuration.TemplateKey, $"%{search.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(templateKey))
            query = query.Where(configuration => configuration.TemplateKey == templateKey.Trim());

        if (studyGroupId.HasValue)
            query = query.Where(configuration => configuration.StudyGroupId == studyGroupId.Value);

        if (isActive.HasValue)
            query = query.Where(configuration => configuration.IsActive == isActive.Value);

        return await query.OrderBy(configuration => configuration.TemplateKey).ThenBy(configuration => configuration.Id).ToListAsync(cancellationToken);
    }

    public async Task<ExportConfiguration?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.ExportConfigurations
            .Include(configuration => configuration.ActiveVersion)
            .Include(configuration => configuration.ExportConfigurationVersions)
            .FirstOrDefaultAsync(configuration => configuration.Id == id, cancellationToken);

    public async Task AddAsync(ExportConfiguration configuration, CancellationToken cancellationToken)
        => await context.ExportConfigurations.AddAsync(configuration, cancellationToken);

    public void Update(ExportConfiguration configuration)
        => context.ExportConfigurations.Update(configuration);

    public void Delete(ExportConfiguration configuration)
        => context.ExportConfigurations.Remove(configuration);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
