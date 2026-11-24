using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ExportConfigurations;

public sealed record GetExportConfigurationByIdQuery(int Id) : IRequest<ExportConfiguration?>;

public sealed class GetExportConfigurationByIdHandler(IExportConfigurationRepository repository) : IRequestHandler<GetExportConfigurationByIdQuery, ExportConfiguration?>
{
    public async Task<ExportConfiguration?> Handle(GetExportConfigurationByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}