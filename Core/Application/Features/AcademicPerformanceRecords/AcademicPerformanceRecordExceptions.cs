namespace Application.Features.AcademicPerformanceRecords;

public sealed class AcademicPerformanceRecordNotFoundException(int id)
    : Exception($"Academic performance record with ID {id} was not found.");

public sealed class AcademicPerformanceRecordConflictException(int importBatchId, int sourceRowNumber)
    : Exception($"Academic performance record with import batch ID {importBatchId} and source row number {sourceRowNumber} already exists.");