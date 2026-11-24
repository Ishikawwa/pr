
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAcademicPerformanceRecordRepository
{
    Task<IReadOnlyList<AcademicPerformanceRecord>> GetAllAsync(
        int? importBatchId,
        int? sourceRowNumber,
        int? disciplineId,
        int? organizationId,
        int? semesterId,
        int? studentId,
        int? studyGroupId,
        int? suborganizationId,
        CancellationToken cancellationToken);

    Task<AcademicPerformanceRecord?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<bool> BatchRowExistsAsync(int importBatchId, int sourceRowNumber, int? excludingId, CancellationToken cancellationToken);

    Task<AcademicPerformanceRecord> AddAsync(AcademicPerformanceRecord record, CancellationToken cancellationToken);

    Task UpdateAsync(AcademicPerformanceRecord record, CancellationToken cancellationToken);

    Task DeleteAsync(AcademicPerformanceRecord record, CancellationToken cancellationToken);
}
