using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Disciplines;

public sealed record DeleteDisciplineCommand(int Id) : IRequest<bool>;

public sealed class DeleteDisciplineHandler(IDisciplineRepository repository) : IRequestHandler<DeleteDisciplineCommand, bool>
{
    public async Task<bool> Handle(DeleteDisciplineCommand request, CancellationToken cancellationToken)
    {
        Discipline? discipline = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (discipline is null)
            return false;

        repository.Delete(discipline);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
