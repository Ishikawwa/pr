using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.ImportBatches;

public sealed record DeleteImportBatchCommand(int Id) : IRequest<bool>;

public sealed class DeleteImportBatchHandler(IImportBatchRepository repository) : IRequestHandler<DeleteImportBatchCommand, bool>
{
    public async Task<bool> Handle(DeleteImportBatchCommand request, CancellationToken cancellationToken)
    {
        ImportBatch? importBatch = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (importBatch is null)
            return false;

        repository.Delete(importBatch);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}