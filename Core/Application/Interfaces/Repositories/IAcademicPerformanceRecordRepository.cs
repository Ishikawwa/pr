using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAcademicPerformanceRecordRepository
{
    Task<IReadOnlyList<AcademicPerformanceRecord>> GetAllAsync(CancellationToken cancellationToken);

    Task<AcademicPerformanceRecord?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<bool> BatchRowExistsAsync(int importBatchId, int sourceRowNumber, int? excludingId, CancellationToken cancellationToken);

    Task<AcademicPerformanceRecord> AddAsync(AcademicPerformanceRecord record, CancellationToken cancellationToken);

    Task UpdateAsync(AcademicPerformanceRecord record, CancellationToken cancellationToken);

    Task DeleteAsync(AcademicPerformanceRecord record, CancellationToken cancellationToken);
}