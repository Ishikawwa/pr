using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ExportConfigurations;

public sealed record CreateExportConfigurationCommand(string TemplateKey, int? StudyGroupId, bool IsActive, int? ActiveVersionId) : IRequest<int>;

public sealed class CreateExportConfigurationHandler(IExportConfigurationRepository repository) : IRequestHandler<CreateExportConfigurationCommand, int>
{
    public async Task<int> Handle(CreateExportConfigurationCommand request, CancellationToken cancellationToken)
    {
        ExportConfiguration configuration = new()
        {
            TemplateKey = request.TemplateKey,
            StudyGroupId = request.StudyGroupId,
            IsActive = request.IsActive,
            ActiveVersionId = request.ActiveVersionId
        };

        await repository.AddAsync(configuration, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return configuration.Id;
    }
}