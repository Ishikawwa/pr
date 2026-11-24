using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class AcademicPerformanceRecordRepository(AppDbContext db) : IAcademicPerformanceRecordRepository
{
    public async Task<IReadOnlyList<AcademicPerformanceRecord>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await db.AcademicPerformanceRecords
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<AcademicPerformanceRecord?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await db.AcademicPerformanceRecords
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> BatchRowExistsAsync(
        int importBatchId,
        int sourceRowNumber,
        int? excludingId,
        CancellationToken cancellationToken)
    {
        return await db.AcademicPerformanceRecords.AnyAsync(
            x => x.ImportBatchId == importBatchId &&
                 x.SourceRowNumber == sourceRowNumber &&
                 (!excludingId.HasValue || x.Id != excludingId.Value),
            cancellationToken);
    }

    public async Task<AcademicPerformanceRecord> AddAsync(
        AcademicPerformanceRecord record,
        CancellationToken cancellationToken)
    {
        db.AcademicPerformanceRecords.Add(record);

        await db.SaveChangesAsync(cancellationToken);

        return record;
    }

    public async Task UpdateAsync(
        AcademicPerformanceRecord record,
        CancellationToken cancellationToken)
    {
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        AcademicPerformanceRecord record,
        CancellationToken cancellationToken)
    {
        db.AcademicPerformanceRecords.Remove(record);

        await db.SaveChangesAsync(cancellationToken);
    }
}