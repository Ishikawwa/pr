using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.FgosDisciplineMappings;

public sealed record PatchFgosDisciplineMappingCommand(int Id, int? DisciplineId, int? FgosDisciplineId) : IRequest<bool>;

public sealed class PatchFgosDisciplineMappingHandler(IFgosDisciplineMappingRepository repository) : IRequestHandler<PatchFgosDisciplineMappingCommand, bool>
{
    public async Task<bool> Handle(PatchFgosDisciplineMappingCommand request, CancellationToken cancellationToken)
    {
        FgosDisciplineMapping? mapping = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (mapping is null)
            return false;

        if (request.DisciplineId.HasValue)
            mapping.DisciplineId = request.DisciplineId.Value;

        if (request.FgosDisciplineId.HasValue)
            mapping.FgosDisciplineId = request.FgosDisciplineId.Value;

        repository.Update(mapping);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
