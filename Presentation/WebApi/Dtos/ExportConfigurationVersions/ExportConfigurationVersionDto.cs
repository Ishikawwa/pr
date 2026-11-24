namespace WebApi.Dtos.ExportConfigurationVersions;

public sealed record ExportConfigurationVersionDto(
    int Id,
    int ConfigurationId,
    DateTime CreatedAt,
    string Name,
    string Settings,
    int VersionNumber,
    string? ChangeNote);