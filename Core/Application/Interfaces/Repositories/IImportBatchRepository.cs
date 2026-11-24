using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IImportBatchRepository
{
    Task<IReadOnlyList<ImportBatch>> GetAllAsync(string? semester, string? sourceFileName, DateTime? importedFrom, DateTime? importedTo, int? rowCount, CancellationToken cancellationToken);
    Task<ImportBatch?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(ImportBatch importBatch, CancellationToken cancellationToken);
    void Update(ImportBatch importBatch);
    void Delete(ImportBatch importBatch);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}