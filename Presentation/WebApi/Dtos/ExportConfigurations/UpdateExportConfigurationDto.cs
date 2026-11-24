namespace WebApi.Dtos.ExportConfigurations;

public sealed record UpdateExportConfigurationDto(
    string TemplateKey,
    int? StudyGroupId,
    bool IsActive,
    int? ActiveVersionId);