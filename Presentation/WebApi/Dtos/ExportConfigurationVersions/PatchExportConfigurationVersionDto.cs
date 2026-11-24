namespace WebApi.Dtos.ExportConfigurationVersions;

public sealed record PatchExportConfigurationVersionDto(
    int? ConfigurationId,
    string? Name,
    string? Settings,
    int? VersionNumber,
    string? ChangeNote);