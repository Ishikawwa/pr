using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ExportConfigurationVersions;

public sealed record GetExportConfigurationVersionByIdQuery(int Id) : IRequest<ExportConfigurationVersion?>;

public sealed class GetExportConfigurationVersionByIdHandler(IExportConfigurationVersionRepository repository) : IRequestHandler<GetExportConfigurationVersionByIdQuery, ExportConfigurationVersion?>
{
    public async Task<ExportConfigurationVersion?> Handle(GetExportConfigurationVersionByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}