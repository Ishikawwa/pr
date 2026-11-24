using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Behaviour.ExportTemplateVersions;

public sealed record DeleteExportTemplateVersionCommand(int Id) : IRequest<bool>;

public sealed class DeleteExportTemplateVersionHandler(IExportTemplateVersionRepository repository) : IRequestHandler<DeleteExportTemplateVersionCommand, bool>
{
    public async Task<bool> Handle(DeleteExportTemplateVersionCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.ExportTemplateVersion? version = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (version is null)
            return false;

        repository.Delete(version);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}