using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ExportConfigurationVersions;

public sealed record GetExportConfigurationVersionsQuery(string? Search, int? ConfigurationId, int? VersionNumber) : IRequest<IReadOnlyList<ExportConfigurationVersion>>;

public sealed class GetExportConfigurationVersionsHandler(IExportConfigurationVersionRepository repository) : IRequestHandler<GetExportConfigurationVersionsQuery, IReadOnlyList<ExportConfigurationVersion>>
{
    public async Task<IReadOnlyList<ExportConfigurationVersion>> Handle(GetExportConfigurationVersionsQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.Search, request.ConfigurationId, request.VersionNumber, cancellationToken);
}