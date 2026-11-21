using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.FgosDisciplines;

public sealed record DeleteFgosDisciplineCommand(int Id) : IRequest<bool>;

public sealed class DeleteFgosDisciplineHandler(IFgosDisciplineRepository repository) : IRequestHandler<DeleteFgosDisciplineCommand, bool>
{
    public async Task<bool> Handle(DeleteFgosDisciplineCommand request, CancellationToken cancellationToken)
    {
        FgosDiscipline? fgosDiscipline = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (fgosDiscipline is null)
            return false;

        repository.Delete(fgosDiscipline);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
