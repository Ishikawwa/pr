namespace WebApi.Dtos.ImportBatches;

public sealed record UpdateImportBatchDto(DateTime ImportedAt, int RowCount, string? Semester, string SourceFileName);