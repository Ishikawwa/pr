using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ExportTemplateVersions;

public sealed record GetExportTemplateVersionByIdQuery(int Id) : IRequest<ExportTemplateVersion?>;

public sealed class GetExportTemplateVersionByIdHandler(IExportTemplateVersionRepository repository) : IRequestHandler<GetExportTemplateVersionByIdQuery, ExportTemplateVersion?>
{
    public async Task<ExportTemplateVersion?> Handle(GetExportTemplateVersionByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}