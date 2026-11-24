using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Disciplines;

public sealed record PatchDisciplineCommand(int Id, string? Code, string? Name, int? SuborganizationId) : IRequest<bool>;

public sealed class PatchDisciplineHandler(IDisciplineRepository repository) : IRequestHandler<PatchDisciplineCommand, bool>
{
    public async Task<bool> Handle(PatchDisciplineCommand request, CancellationToken cancellationToken)
    {
        Discipline? discipline = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (discipline is null)
            return false;

        if (request.Code is not null)
            discipline.Code = request.Code;

        if (request.Name is not null)
            discipline.Name = request.Name;

        if (request.SuborganizationId.HasValue)
            discipline.SuborganizationId = request.SuborganizationId.Value;

        repository.Update(discipline);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
