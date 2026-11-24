using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ExportTemplateVersions;

public sealed record CreateExportTemplateVersionCommand(
    int? BasedOnVersionId,
    string ContentBase64,
    string Format,
    bool IsActive,
    string Name,
    string OriginalFileName,
    int? StudyGroupId,
    string TemplateKey,
    int VersionNumber) : IRequest<int>;

public sealed class CreateExportTemplateVersionHandler(IExportTemplateVersionRepository repository) : IRequestHandler<CreateExportTemplateVersionCommand, int>
{
    public async Task<int> Handle(CreateExportTemplateVersionCommand request, CancellationToken cancellationToken)
    {
        ExportTemplateVersion version = new()
        {
            BasedOnVersionId = request.BasedOnVersionId,
            ContentBase64 = request.ContentBase64,
            Format = request.Format,
            IsActive = request.IsActive,
            Name = request.Name,
            OriginalFileName = request.OriginalFileName,
            StudyGroupId = request.StudyGroupId,
            TemplateKey = request.TemplateKey,
            VersionNumber = request.VersionNumber
        };

        await repository.AddAsync(version, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return version.Id;
    }
}