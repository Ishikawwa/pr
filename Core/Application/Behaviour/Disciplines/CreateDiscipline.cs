using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Disciplines;

public sealed record CreateDisciplineCommand(string Code, string Name, int SuborganizationId) : IRequest<int>;

public sealed class CreateDisciplineHandler(IDisciplineRepository repository) : IRequestHandler<CreateDisciplineCommand, int>
{
    public async Task<int> Handle(CreateDisciplineCommand request, CancellationToken cancellationToken)
    {
        Discipline discipline = new()
        {
            Code = request.Code,
            Name = request.Name,
            SuborganizationId = request.SuborganizationId
        };

        await repository.AddAsync(discipline, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return discipline.Id;
    }
}
