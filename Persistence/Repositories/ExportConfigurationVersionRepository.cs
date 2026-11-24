
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class ExportConfigurationVersionRepository(AppDbContext context) : IExportConfigurationVersionRepository
{
    public async Task<IReadOnlyList<ExportConfigurationVersion>> GetAllAsync(string? search, int? configurationId, int? versionNumber, CancellationToken cancellationToken)
    {
        IQueryable<ExportConfigurationVersion> query = context.ExportConfigurationVersions.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(version => EF.Functions.ILike(version.Name, $"%{search.Trim()}%") ||
                (version.ChangeNote != null && EF.Functions.ILike(version.ChangeNote, $"%{search.Trim()}%")));

        if (configurationId.HasValue)
            query = query.Where(version => version.ConfigurationId == configurationId.Value);

        if (versionNumber.HasValue)
            query = query.Where(version => version.VersionNumber == versionNumber.Value);

        return await query.OrderBy(version => version.ConfigurationId).ThenByDescending(version => version.VersionNumber).ToListAsync(cancellationToken);
    }

    public async Task<ExportConfigurationVersion?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.ExportConfigurationVersions
            .FirstOrDefaultAsync(version => version.Id == id, cancellationToken);

    public async Task AddAsync(ExportConfigurationVersion version, CancellationToken cancellationToken)
        => await context.ExportConfigurationVersions.AddAsync(version, cancellationToken);

    public void Update(ExportConfigurationVersion version)
        => context.ExportConfigurationVersions.Update(version);

    public void Delete(ExportConfigurationVersion version)
        => context.ExportConfigurationVersions.Remove(version);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
