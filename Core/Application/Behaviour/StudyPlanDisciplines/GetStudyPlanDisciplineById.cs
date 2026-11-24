using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPlanDisciplines;

public sealed record GetStudyPlanDisciplineByIdQuery(int Id) : IRequest<StudyPlanDiscipline?>;

public sealed class GetStudyPlanDisciplineByIdHandler(IStudyPlanDisciplineRepository repository) : IRequestHandler<GetStudyPlanDisciplineByIdQuery, StudyPlanDiscipline?>
{
    public async Task<StudyPlanDiscipline?> Handle(GetStudyPlanDisciplineByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}
