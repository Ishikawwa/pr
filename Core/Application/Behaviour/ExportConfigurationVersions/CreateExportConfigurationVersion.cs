using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ExportConfigurationVersions;

public sealed record CreateExportConfigurationVersionCommand(int ConfigurationId, string Name, string Settings, int VersionNumber, string? ChangeNote) : IRequest<int>;

public sealed class CreateExportConfigurationVersionHandler(IExportConfigurationVersionRepository repository) : IRequestHandler<CreateExportConfigurationVersionCommand, int>
{
    public async Task<int> Handle(CreateExportConfigurationVersionCommand request, CancellationToken cancellationToken)
    {
        ExportConfigurationVersion version = new()
        {
            ConfigurationId = request.ConfigurationId,
            Name = request.Name,
            Settings = request.Settings,
            VersionNumber = request.VersionNumber,
            ChangeNote = request.ChangeNote
        };

        await repository.AddAsync(version, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return version.Id;
    }
}