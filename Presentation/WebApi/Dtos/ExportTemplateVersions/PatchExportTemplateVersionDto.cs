namespace WebApi.Dtos.ExportTemplateVersions;

public sealed record PatchExportTemplateVersionDto(
    int? BasedOnVersionId,
    string? ContentBase64,
    string? Format,
    bool? IsActive,
    string? Name,
    string? OriginalFileName,
    int? StudyGroupId,
    string? TemplateKey,
    int? VersionNumber);