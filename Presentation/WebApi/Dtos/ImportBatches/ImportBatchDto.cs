namespace WebApi.Dtos.ImportBatches;

public sealed record ImportBatchDto(int Id, DateTime ImportedAt, int RowCount, string? Semester, string SourceFileName);