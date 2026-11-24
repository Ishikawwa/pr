namespace WebApi.Dtos.ExportConfigurationVersions;

public sealed record UpdateExportConfigurationVersionDto(
    int ConfigurationId,
    string Name,
    string Settings,
    int VersionNumber,
    string? ChangeNote);