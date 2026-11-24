using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ExportTemplateVersions;

public sealed record GetExportTemplateVersionsQuery(string? Search, string? TemplateKey, string? Format, int? StudyGroupId, bool? IsActive) : IRequest<IReadOnlyList<ExportTemplateVersion>>;

public sealed class GetExportTemplateVersionsHandler(IExportTemplateVersionRepository repository) : IRequestHandler<GetExportTemplateVersionsQuery, IReadOnlyList<ExportTemplateVersion>>
{
    public async Task<IReadOnlyList<ExportTemplateVersion>> Handle(GetExportTemplateVersionsQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.Search, request.TemplateKey, request.Format, request.StudyGroupId, request.IsActive, cancellationToken);
}