using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ImportBatches;

public sealed record UpdateImportBatchCommand(int Id, DateTime ImportedAt, int RowCount, string? Semester, string SourceFileName) : IRequest<bool>;

public sealed class UpdateImportBatchHandler(IImportBatchRepository repository) : IRequestHandler<UpdateImportBatchCommand, bool>
{
    public async Task<bool> Handle(UpdateImportBatchCommand request, CancellationToken cancellationToken)
    {
        ImportBatch? importBatch = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (importBatch is null)
            return false;

        importBatch.ImportedAt = request.ImportedAt;
        importBatch.RowCount = request.RowCount;
        importBatch.Semester = request.Semester;
        importBatch.SourceFileName = request.SourceFileName;

        repository.Update(importBatch);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}