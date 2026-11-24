using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Behaviour.ExportTemplateVersions;

public sealed record PatchExportTemplateVersionCommand(
    int Id,
    int? BasedOnVersionId,
    string? ContentBase64,
    string? Format,
    bool? IsActive,
    string? Name,
    string? OriginalFileName,
    int? StudyGroupId,
    string? TemplateKey,
    int? VersionNumber) : IRequest<bool>;

public sealed class PatchExportTemplateVersionHandler(IExportTemplateVersionRepository repository) : IRequestHandler<PatchExportTemplateVersionCommand, bool>
{
    public async Task<bool> Handle(PatchExportTemplateVersionCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.ExportTemplateVersion? version = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (version is null)
            return false;

        if (request.BasedOnVersionId.HasValue)
            version.BasedOnVersionId = request.BasedOnVersionId.Value;

        if (request.ContentBase64 is not null)
            version.ContentBase64 = request.ContentBase64;

        if (request.Format is not null)
            version.Format = request.Format;

        if (request.IsActive.HasValue)
            version.IsActive = request.IsActive.Value;

        if (request.Name is not null)
            version.Name = request.Name;

        if (request.OriginalFileName is not null)
            version.OriginalFileName = request.OriginalFileName;

        if (request.StudyGroupId.HasValue)
            version.StudyGroupId = request.StudyGroupId.Value;

        if (request.TemplateKey is not null)
            version.TemplateKey = request.TemplateKey;

        if (request.VersionNumber.HasValue)
            version.VersionNumber = request.VersionNumber.Value;

        repository.Update(version);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}