namespace WebApi.Dtos.ImportBatches;

public sealed record PatchImportBatchDto(DateTime? ImportedAt, int? RowCount, string? Semester, string? SourceFileName);