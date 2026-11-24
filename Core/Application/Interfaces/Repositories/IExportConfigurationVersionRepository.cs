using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IExportConfigurationVersionRepository
{
    Task<IReadOnlyList<ExportConfigurationVersion>> GetAllAsync(string? search, int? configurationId, int? versionNumber, CancellationToken cancellationToken);

    Task<ExportConfigurationVersion?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task AddAsync(ExportConfigurationVersion version, CancellationToken cancellationToken);

    void Update(ExportConfigurationVersion version);

    void Delete(ExportConfigurationVersion version);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}