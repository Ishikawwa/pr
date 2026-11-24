using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Behaviour.ExportConfigurationVersions;

public sealed record DeleteExportConfigurationVersionCommand(int Id) : IRequest<bool>;

public sealed class DeleteExportConfigurationVersionHandler(IExportConfigurationVersionRepository repository) : IRequestHandler<DeleteExportConfigurationVersionCommand, bool>
{
    public async Task<bool> Handle(DeleteExportConfigurationVersionCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.ExportConfigurationVersion? version = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (version is null)
            return false;

        repository.Delete(version);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}