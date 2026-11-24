using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IExportConfigurationRepository
{
    Task<IReadOnlyList<ExportConfiguration>> GetAllAsync(string? search, string? templateKey, int? studyGroupId, bool? isActive, CancellationToken cancellationToken);

    Task<ExportConfiguration?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task AddAsync(ExportConfiguration configuration, CancellationToken cancellationToken);

    void Update(ExportConfiguration configuration);

    void Delete(ExportConfiguration configuration);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}