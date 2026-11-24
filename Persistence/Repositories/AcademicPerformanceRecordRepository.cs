using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class AcademicPerformanceRecordRepository(AppDbContext db) : IAcademicPerformanceRecordRepository
{
    public async Task<IReadOnlyList<AcademicPerformanceRecord>> GetAllAsync(
        int? importBatchId,
        int? sourceRowNumber,
        int? disciplineId,
        int? organizationId,
        int? semesterId,
        int? studentId,
        int? studyGroupId,
        int? suborganizationId,
        CancellationToken cancellationToken)
    {
        IQueryable<AcademicPerformanceRecord> query = db.AcademicPerformanceRecords.AsNoTracking();

        if (importBatchId.HasValue)
            query = query.Where(x => x.ImportBatchId == importBatchId.Value);

        if (sourceRowNumber.HasValue)
            query = query.Where(x => x.SourceRowNumber == sourceRowNumber.Value);

        if (disciplineId.HasValue)
            query = query.Where(x => x.DisciplineId == disciplineId.Value);

        if (organizationId.HasValue)
            query = query.Where(x => x.OrganizationId == organizationId.Value);

        if (semesterId.HasValue)
            query = query.Where(x => x.SemesterId == semesterId.Value);

        if (studentId.HasValue)
            query = query.Where(x => x.StudentId == studentId.Value);

        if (studyGroupId.HasValue)
            query = query.Where(x => x.StudyGroupId == studyGroupId.Value);

        if (suborganizationId.HasValue)
            query = query.Where(x => x.SuborganizationId == suborganizationId.Value);

        return await query
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
