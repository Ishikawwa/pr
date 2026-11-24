namespace WebApi.Dtos.ExportConfigurations;

public sealed record CreateExportConfigurationDto(
    string TemplateKey,
    int? StudyGroupId,
    bool IsActive,
    int? ActiveVersionId);