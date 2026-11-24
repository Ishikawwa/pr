
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class ExportTemplateVersionRepository(AppDbContext context) : IExportTemplateVersionRepository
{
    public async Task<IReadOnlyList<ExportTemplateVersion>> GetAllAsync(string? search, string? templateKey, string? format, int? studyGroupId, bool? isActive, CancellationToken cancellationToken)
    {
        IQueryable<ExportTemplateVersion> query = context.ExportTemplateVersions.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(version => EF.Functions.ILike(version.Name, $"%{search.Trim()}%") ||
                EF.Functions.ILike(version.OriginalFileName, $"%{search.Trim()}%") ||
                EF.Functions.ILike(version.TemplateKey, $"%{search.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(templateKey))
            query = query.Where(version => version.TemplateKey == templateKey.Trim());

        if (!string.IsNullOrWhiteSpace(format))
            query = query.Where(version => version.Format == format.Trim());

        if (studyGroupId.HasValue)
            query = query.Where(version => version.StudyGroupId == studyGroupId.Value);

        if (isActive.HasValue)
            query = query.Where(version => version.IsActive == isActive.Value);

        return await query.OrderBy(version => version.TemplateKey).ThenByDescending(version => version.VersionNumber).ToListAsync(cancellationToken);
    }

    public async Task<ExportTemplateVersion?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.ExportTemplateVersions
            .Include(version => version.BasedOnVersion)
            .FirstOrDefaultAsync(version => version.Id == id, cancellationToken);

    public async Task AddAsync(ExportTemplateVersion version, CancellationToken cancellationToken)
        => await context.ExportTemplateVersions.AddAsync(version, cancellationToken);

    public void Update(ExportTemplateVersion version)
        => context.ExportTemplateVersions.Update(version);

    public void Delete(ExportTemplateVersion version)
        => context.ExportTemplateVersions.Remove(version);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
