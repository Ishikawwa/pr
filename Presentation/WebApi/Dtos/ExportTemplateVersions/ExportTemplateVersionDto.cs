namespace WebApi.Dtos.ExportTemplateVersions;

public sealed record ExportTemplateVersionDto(
    int Id,
    int? BasedOnVersionId,
    string ContentBase64,
    DateTime CreatedAt,
    string Format,
    bool IsActive,
    string Name,
    string OriginalFileName,
    int? StudyGroupId,
    string TemplateKey,
    int VersionNumber);