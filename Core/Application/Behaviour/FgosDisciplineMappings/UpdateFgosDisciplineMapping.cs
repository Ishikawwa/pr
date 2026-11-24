using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.FgosDisciplineMappings;

public sealed record UpdateFgosDisciplineMappingCommand(int Id, int DisciplineId, int FgosDisciplineId) : IRequest<bool>;

public sealed class UpdateFgosDisciplineMappingHandler(IFgosDisciplineMappingRepository repository) : IRequestHandler<UpdateFgosDisciplineMappingCommand, bool>
{
    public async Task<bool> Handle(UpdateFgosDisciplineMappingCommand request, CancellationToken cancellationToken)
    {
        FgosDisciplineMapping? mapping = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (mapping is null)
            return false;

        mapping.DisciplineId = request.DisciplineId;
        mapping.FgosDisciplineId = request.FgosDisciplineId;

        repository.Update(mapping);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
