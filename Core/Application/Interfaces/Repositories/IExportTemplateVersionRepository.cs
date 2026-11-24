using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IExportTemplateVersionRepository
{
    Task<IReadOnlyList<ExportTemplateVersion>> GetAllAsync(string? search, string? templateKey, string? format, int? studyGroupId, bool? isActive, CancellationToken cancellationToken);

    Task<ExportTemplateVersion?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task AddAsync(ExportTemplateVersion version, CancellationToken cancellationToken);

    void Update(ExportTemplateVersion version);

    void Delete(ExportTemplateVersion version);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}