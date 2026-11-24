using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ExportConfigurations;

public sealed record GetExportConfigurationsQuery(string? Search, string? TemplateKey, int? StudyGroupId, bool? IsActive) : IRequest<IReadOnlyList<ExportConfiguration>>;

public sealed class GetExportConfigurationsHandler(IExportConfigurationRepository repository) : IRequestHandler<GetExportConfigurationsQuery, IReadOnlyList<ExportConfiguration>>
{
    public async Task<IReadOnlyList<ExportConfiguration>> Handle(GetExportConfigurationsQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.Search, request.TemplateKey, request.StudyGroupId, request.IsActive, cancellationToken);
}