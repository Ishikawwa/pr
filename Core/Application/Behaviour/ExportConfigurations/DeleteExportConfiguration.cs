using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Behaviour.ExportConfigurations;

public sealed record DeleteExportConfigurationCommand(int Id) : IRequest<bool>;

public sealed class DeleteExportConfigurationHandler(IExportConfigurationRepository repository) : IRequestHandler<DeleteExportConfigurationCommand, bool>
{
    public async Task<bool> Handle(DeleteExportConfigurationCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.ExportConfiguration? configuration = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (configuration is null)
            return false;

        repository.Delete(configuration);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}