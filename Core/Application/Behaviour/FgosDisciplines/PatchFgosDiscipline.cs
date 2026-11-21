using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.FgosDisciplines;

public sealed record PatchFgosDisciplineCommand(int Id, string? Code, string? Name, int? StudyProgramId) : IRequest<bool>;

public sealed class PatchFgosDisciplineHandler(IFgosDisciplineRepository repository) : IRequestHandler<PatchFgosDisciplineCommand, bool>
{
    public async Task<bool> Handle(PatchFgosDisciplineCommand request, CancellationToken cancellationToken)
    {
        FgosDiscipline? fgosDiscipline = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (fgosDiscipline is null)
            return false;

        if (request.Code is not null)
            fgosDiscipline.Code = request.Code;

        if (request.Name is not null)
            fgosDiscipline.Name = request.Name;

        if (request.StudyProgramId.HasValue)
            fgosDiscipline.StudyProgramId = request.StudyProgramId.Value;

        repository.Update(fgosDiscipline);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
