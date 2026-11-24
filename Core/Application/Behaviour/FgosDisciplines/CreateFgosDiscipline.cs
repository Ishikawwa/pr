using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.FgosDisciplines;

public sealed record CreateFgosDisciplineCommand(string Code, string Name, int StudyProgramId) : IRequest<int>;

public sealed class CreateFgosDisciplineHandler(IFgosDisciplineRepository repository) : IRequestHandler<CreateFgosDisciplineCommand, int>
{
    public async Task<int> Handle(CreateFgosDisciplineCommand request, CancellationToken cancellationToken)
    {
        FgosDiscipline fgosDiscipline = new()
        {
            Code = request.Code,
            Name = request.Name,
            StudyProgramId = request.StudyProgramId
        };

        await repository.AddAsync(fgosDiscipline, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return fgosDiscipline.Id;
    }
}
