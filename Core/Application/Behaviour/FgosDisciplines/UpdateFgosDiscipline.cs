using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.FgosDisciplines;

public sealed record UpdateFgosDisciplineCommand(int Id, string Code, string Name, int StudyProgramId) : IRequest<bool>;

public sealed class UpdateFgosDisciplineHandler(IFgosDisciplineRepository repository) : IRequestHandler<UpdateFgosDisciplineCommand, bool>
{
    public async Task<bool> Handle(UpdateFgosDisciplineCommand request, CancellationToken cancellationToken)
    {
        FgosDiscipline? fgosDiscipline = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (fgosDiscipline is null)
            return false;

        fgosDiscipline.Code = request.Code;
        fgosDiscipline.Name = request.Name;
        fgosDiscipline.StudyProgramId = request.StudyProgramId;

        repository.Update(fgosDiscipline);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
