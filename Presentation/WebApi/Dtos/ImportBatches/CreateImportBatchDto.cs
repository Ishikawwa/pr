namespace WebApi.Dtos.ImportBatches;

public sealed record CreateImportBatchDto(string? Semester, string SourceFileName, int RowCount);