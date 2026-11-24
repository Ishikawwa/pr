using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Disciplines;

public sealed record UpdateDisciplineCommand(int Id, string Code, string Name, int SuborganizationId) : IRequest<bool>;

public sealed class UpdateDisciplineHandler(IDisciplineRepository repository) : IRequestHandler<UpdateDisciplineCommand, bool>
{
    public async Task<bool> Handle(UpdateDisciplineCommand request, CancellationToken cancellationToken)
    {
        Discipline? discipline = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (discipline is null)
            return false;

        discipline.Code = request.Code;
        discipline.Name = request.Name;
        discipline.SuborganizationId = request.SuborganizationId;

        repository.Update(discipline);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
