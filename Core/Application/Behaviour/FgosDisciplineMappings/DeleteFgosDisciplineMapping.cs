using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.FgosDisciplineMappings;

public sealed record DeleteFgosDisciplineMappingCommand(int Id) : IRequest<bool>;

public sealed class DeleteFgosDisciplineMappingHandler(IFgosDisciplineMappingRepository repository) : IRequestHandler<DeleteFgosDisciplineMappingCommand, bool>
{
    public async Task<bool> Handle(DeleteFgosDisciplineMappingCommand request, CancellationToken cancellationToken)
    {
        FgosDisciplineMapping? mapping = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (mapping is null)
            return false;

        repository.Delete(mapping);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
