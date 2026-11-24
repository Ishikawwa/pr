using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Behaviour.ExportConfigurationVersions;

public sealed record PatchExportConfigurationVersionCommand(int Id, int? ConfigurationId, string? Name, string? Settings, int? VersionNumber, string? ChangeNote) : IRequest<bool>;

public sealed class PatchExportConfigurationVersionHandler(IExportConfigurationVersionRepository repository) : IRequestHandler<PatchExportConfigurationVersionCommand, bool>
{
    public async Task<bool> Handle(PatchExportConfigurationVersionCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.ExportConfigurationVersion? version = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (version is null)
            return false;

        if (request.ConfigurationId.HasValue)
            version.ConfigurationId = request.ConfigurationId.Value;

        if (request.Name is not null)
            version.Name = request.Name;

        if (request.Settings is not null)
            version.Settings = request.Settings;

        if (request.VersionNumber.HasValue)
            version.VersionNumber = request.VersionNumber.Value;

        if (request.ChangeNote is not null)
            version.ChangeNote = request.ChangeNote;

        repository.Update(version);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}