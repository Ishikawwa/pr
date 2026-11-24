namespace WebApi.Dtos.ExportConfigurations;

public sealed record PatchExportConfigurationDto(
    string? TemplateKey,
    int? StudyGroupId,
    bool? IsActive,
    int? ActiveVersionId);