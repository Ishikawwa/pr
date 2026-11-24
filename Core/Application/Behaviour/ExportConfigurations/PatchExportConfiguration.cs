using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Behaviour.ExportConfigurations;

public sealed record PatchExportConfigurationCommand(int Id, string? TemplateKey, int? StudyGroupId, bool? IsActive, int? ActiveVersionId) : IRequest<bool>;

public sealed class PatchExportConfigurationHandler(IExportConfigurationRepository repository) : IRequestHandler<PatchExportConfigurationCommand, bool>
{
    public async Task<bool> Handle(PatchExportConfigurationCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.ExportConfiguration? configuration = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (configuration is null)
            return false;

        if (request.TemplateKey is not null)
            configuration.TemplateKey = request.TemplateKey;

        if (request.StudyGroupId.HasValue)
            configuration.StudyGroupId = request.StudyGroupId.Value;

        if (request.IsActive.HasValue)
            configuration.IsActive = request.IsActive.Value;

        if (request.ActiveVersionId.HasValue)
            configuration.ActiveVersionId = request.ActiveVersionId.Value;

        configuration.UpdatedAt = DateTime.UtcNow;

        repository.Update(configuration);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}