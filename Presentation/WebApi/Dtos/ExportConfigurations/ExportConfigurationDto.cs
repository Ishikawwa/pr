namespace WebApi.Dtos.ExportConfigurations;

public sealed record ExportConfigurationDto(
    int Id,
    int? ActiveVersionId,
    DateTime CreatedAt,
    bool IsActive,
    int? StudyGroupId,
    string TemplateKey,
    DateTime UpdatedAt);