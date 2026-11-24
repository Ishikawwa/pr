using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPlanDisciplines;

public sealed record GetStudyPlanDisciplinesQuery(int? StudyPlanId, int? FgosDisciplineId, int? SemesterNumber, string? ControlForm, string? FgosDisciplineCode, string? FgosDisciplineName) : IRequest<IReadOnlyList<StudyPlanDiscipline>>;

public sealed class GetStudyPlanDisciplinesHandler(IStudyPlanDisciplineRepository repository) : IRequestHandler<GetStudyPlanDisciplinesQuery, IReadOnlyList<StudyPlanDiscipline>>
{
    public async Task<IReadOnlyList<StudyPlanDiscipline>> Handle(GetStudyPlanDisciplinesQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.StudyPlanId, request.FgosDisciplineId, request.SemesterNumber, request.ControlForm, request.FgosDisciplineCode, request.FgosDisciplineName, cancellationToken);
}
