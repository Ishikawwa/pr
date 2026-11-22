using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPlanDisciplines;

public sealed record UpdateStudyPlanDisciplineCommand(int Id, int StudyPlanId, int FgosDisciplineId, int SemesterNumber, int? PlannedHours, string? ControlForm) : IRequest<bool>;

public sealed class UpdateStudyPlanDisciplineHandler(IStudyPlanDisciplineRepository repository) : IRequestHandler<UpdateStudyPlanDisciplineCommand, bool>
{
    public async Task<bool> Handle(UpdateStudyPlanDisciplineCommand request, CancellationToken cancellationToken)
    {
        StudyPlanDiscipline? studyPlanDiscipline = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studyPlanDiscipline is null)
            return false;

        studyPlanDiscipline.StudyPlanId = request.StudyPlanId;
        studyPlanDiscipline.FgosDisciplineId = request.FgosDisciplineId;
        studyPlanDiscipline.SemesterNumber = request.SemesterNumber;
        studyPlanDiscipline.PlannedHours = request.PlannedHours;
        studyPlanDiscipline.ControlForm = request.ControlForm;

        repository.Update(studyPlanDiscipline);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
