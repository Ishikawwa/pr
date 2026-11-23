using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ImportBatches;

public sealed record GetImportBatchByIdQuery(int Id) : IRequest<ImportBatch?>;

public sealed class GetImportBatchByIdHandler(IImportBatchRepository repository) : IRequestHandler<GetImportBatchByIdQuery, ImportBatch?>
{
    public async Task<ImportBatch?> Handle(GetImportBatchByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}