using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ImportBatches;

public sealed record GetImportBatchesQuery(string? Semester, string? SourceFileName, DateTime? ImportedFrom, DateTime? ImportedTo, int? RowCount) : IRequest<IReadOnlyList<ImportBatch>>;

public sealed class GetImportBatchesHandler(IImportBatchRepository repository) : IRequestHandler<GetImportBatchesQuery, IReadOnlyList<ImportBatch>>
{
    public async Task<IReadOnlyList<ImportBatch>> Handle(GetImportBatchesQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.Semester, request.SourceFileName, request.ImportedFrom, request.ImportedTo, request.RowCount, cancellationToken);
}