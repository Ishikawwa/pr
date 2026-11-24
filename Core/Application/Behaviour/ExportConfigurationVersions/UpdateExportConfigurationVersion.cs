using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Behaviour.ExportConfigurationVersions;

public sealed record UpdateExportConfigurationVersionCommand(int Id, int ConfigurationId, string Name, string Settings, int VersionNumber, string? ChangeNote) : IRequest<bool>;

public sealed class UpdateExportConfigurationVersionHandler(IExportConfigurationVersionRepository repository) : IRequestHandler<UpdateExportConfigurationVersionCommand, bool>
{
    public async Task<bool> Handle(UpdateExportConfigurationVersionCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.ExportConfigurationVersion? version = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (version is null)
            return false;

        version.ConfigurationId = request.ConfigurationId;
        version.Name = request.Name;
        version.Settings = request.Settings;
        version.VersionNumber = request.VersionNumber;
        version.ChangeNote = request.ChangeNote;

        repository.Update(version);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}