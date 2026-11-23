using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class ImportBatchRepository(AppDbContext context) : IImportBatchRepository
{
    public async Task<IReadOnlyList<ImportBatch>> GetAllAsync(string? semester, string? sourceFileName, DateTime? importedFrom, DateTime? importedTo, int? rowCount, CancellationToken cancellationToken)
    {
        IQueryable<ImportBatch> query = context.ImportBatches.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(semester))
            query = query.Where(importBatch => importBatch.Semester != null && EF.Functions.ILike(importBatch.Semester, $"%{semester.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(sourceFileName))
            query = query.Where(importBatch => EF.Functions.ILike(importBatch.SourceFileName, $"%{sourceFileName.Trim()}%"));

        if (importedFrom.HasValue)
            query = query.Where(importBatch => importBatch.ImportedAt >= importedFrom.Value);

        if (importedTo.HasValue)
            query = query.Where(importBatch => importBatch.ImportedAt <= importedTo.Value);

        if (rowCount.HasValue)
            query = query.Where(importBatch => importBatch.RowCount == rowCount.Value);

        return await query.OrderByDescending(importBatch => importBatch.ImportedAt)
            .ThenByDescending(importBatch => importBatch.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<ImportBatch?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.ImportBatches.FirstOrDefaultAsync(importBatch => importBatch.Id == id, cancellationToken);

    public async Task AddAsync(ImportBatch importBatch, CancellationToken cancellationToken)
        => await context.ImportBatches.AddAsync(importBatch, cancellationToken);

    public void Update(ImportBatch importBatch)
        => context.ImportBatches.Update(importBatch);

    public void Delete(ImportBatch importBatch)
        => context.ImportBatches.Remove(importBatch);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}