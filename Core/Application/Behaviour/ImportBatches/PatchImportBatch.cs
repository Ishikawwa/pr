using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ImportBatches;

public sealed record PatchImportBatchCommand(int Id, DateTime? ImportedAt, int? RowCount, string? Semester, string? SourceFileName) : IRequest<bool>;

public sealed class PatchImportBatchHandler(IImportBatchRepository repository) : IRequestHandler<PatchImportBatchCommand, bool>
{
    public async Task<bool> Handle(PatchImportBatchCommand request, CancellationToken cancellationToken)
    {
        ImportBatch? importBatch = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (importBatch is null)
            return false;

        if (request.ImportedAt.HasValue)
            importBatch.ImportedAt = request.ImportedAt.Value;

        if (request.RowCount.HasValue)
            importBatch.RowCount = request.RowCount.Value;

        if (request.Semester is not null)
            importBatch.Semester = request.Semester;

        if (request.SourceFileName is not null)
            importBatch.SourceFileName = request.SourceFileName;

        repository.Update(importBatch);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}