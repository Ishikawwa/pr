
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPlanDisciplines;

public sealed record PatchStudyPlanDisciplineCommand(int Id, int? StudyPlanId, int? FgosDisciplineId, int? SemesterNumber, int? PlannedHours, string? ControlForm) : IRequest<bool>;

public sealed class PatchStudyPlanDisciplineHandler(IStudyPlanDisciplineRepository repository) : IRequestHandler<PatchStudyPlanDisciplineCommand, bool>
{
    public async Task<bool> Handle(PatchStudyPlanDisciplineCommand request, CancellationToken cancellationToken)
    {
        StudyPlanDiscipline? studyPlanDiscipline = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studyPlanDiscipline is null)
            return false;

        if (request.StudyPlanId.HasValue)
            studyPlanDiscipline.StudyPlanId = request.StudyPlanId.Value;

        if (request.FgosDisciplineId.HasValue)
            studyPlanDiscipline.FgosDisciplineId = request.FgosDisciplineId.Value;

        if (request.SemesterNumber.HasValue)
            studyPlanDiscipline.SemesterNumber = request.SemesterNumber.Value;

        if (request.PlannedHours.HasValue)
            studyPlanDiscipline.PlannedHours = request.PlannedHours.Value;

        if (request.ControlForm is not null)
            studyPlanDiscipline.ControlForm = request.ControlForm;

        repository.Update(studyPlanDiscipline);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
