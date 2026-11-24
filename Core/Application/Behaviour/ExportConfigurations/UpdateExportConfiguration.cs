using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Behaviour.ExportConfigurations;

public sealed record UpdateExportConfigurationCommand(int Id, string TemplateKey, int? StudyGroupId, bool IsActive, int? ActiveVersionId) : IRequest<bool>;

public sealed class UpdateExportConfigurationHandler(IExportConfigurationRepository repository) : IRequestHandler<UpdateExportConfigurationCommand, bool>
{
    public async Task<bool> Handle(UpdateExportConfigurationCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.ExportConfiguration? configuration = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (configuration is null)
            return false;

        configuration.TemplateKey = request.TemplateKey;
        configuration.StudyGroupId = request.StudyGroupId;
        configuration.IsActive = request.IsActive;
        configuration.ActiveVersionId = request.ActiveVersionId;
        configuration.UpdatedAt = DateTime.UtcNow;

        repository.Update(configuration);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}