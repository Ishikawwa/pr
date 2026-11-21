using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPlans;

public sealed record GetStudyPlansQuery(string? Name, int? AdmissionBase, int? AdmissionYear, int? StudyProgramId) : IRequest<IReadOnlyList<StudyPlan>>;

public sealed class GetStudyPlansHandler(IStudyPlanRepository repository) : IRequestHandler<GetStudyPlansQuery, IReadOnlyList<StudyPlan>>
{
    public async Task<IReadOnlyList<StudyPlan>> Handle(GetStudyPlansQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.Name, request.AdmissionBase, request.AdmissionYear, request.StudyProgramId, cancellationToken);
}
