using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPlans;

public sealed record GetStudyPlanByIdQuery(int Id) : IRequest<StudyPlan?>;

public sealed class GetStudyPlanByIdHandler(IStudyPlanRepository repository) : IRequestHandler<GetStudyPlanByIdQuery, StudyPlan?>
{
    public async Task<StudyPlan?> Handle(GetStudyPlanByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}
