using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Behaviour.ExportTemplateVersions;

public sealed record UpdateExportTemplateVersionCommand(
    int Id,
    int? BasedOnVersionId,
    string ContentBase64,
    string Format,
    bool IsActive,
    string Name,
    string OriginalFileName,
    int? StudyGroupId,
    string TemplateKey,
    int VersionNumber) : IRequest<bool>;

public sealed class UpdateExportTemplateVersionHandler(IExportTemplateVersionRepository repository) : IRequestHandler<UpdateExportTemplateVersionCommand, bool>
{
    public async Task<bool> Handle(UpdateExportTemplateVersionCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.ExportTemplateVersion? version = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (version is null)
            return false;

        version.BasedOnVersionId = request.BasedOnVersionId;
        version.ContentBase64 = request.ContentBase64;
        version.Format = request.Format;
        version.IsActive = request.IsActive;
        version.Name = request.Name;
        version.OriginalFileName = request.OriginalFileName;
        version.StudyGroupId = request.StudyGroupId;
        version.TemplateKey = request.TemplateKey;
        version.VersionNumber = request.VersionNumber;

        repository.Update(version);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}