namespace WebApi.Dtos.ExportConfigurationVersions;

public sealed record CreateExportConfigurationVersionDto(
    int ConfigurationId,
    string Name,
    string Settings,
    int VersionNumber,
    string? ChangeNote);