using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ImportBatches;

public sealed record CreateImportBatchCommand(string? Semester, string SourceFileName, int RowCount) : IRequest<int>;

public sealed class CreateImportBatchHandler(IImportBatchRepository repository) : IRequestHandler<CreateImportBatchCommand, int>
{
    public async Task<int> Handle(CreateImportBatchCommand request, CancellationToken cancellationToken)
    {
        ImportBatch importBatch = new()
        {
            Semester = request.Semester,
            SourceFileName = request.SourceFileName,
            RowCount = request.RowCount
        };

        await repository.AddAsync(importBatch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return importBatch.Id;
    }
}