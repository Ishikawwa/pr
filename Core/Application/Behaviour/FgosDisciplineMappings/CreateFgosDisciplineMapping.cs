using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.FgosDisciplineMappings;

public sealed record CreateFgosDisciplineMappingCommand(int DisciplineId, int FgosDisciplineId) : IRequest<int>;

public sealed class CreateFgosDisciplineMappingHandler(IFgosDisciplineMappingRepository repository) : IRequestHandler<CreateFgosDisciplineMappingCommand, int>
{
    public async Task<int> Handle(CreateFgosDisciplineMappingCommand request, CancellationToken cancellationToken)
    {
        FgosDisciplineMapping mapping = new()
        {
            DisciplineId = request.DisciplineId,
            FgosDisciplineId = request.FgosDisciplineId
        };

        await repository.AddAsync(mapping, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return mapping.Id;
    }
}
