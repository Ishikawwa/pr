    using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPlanDisciplines;

public sealed record CreateStudyPlanDisciplineCommand(int StudyPlanId, int FgosDisciplineId, int SemesterNumber, int? PlannedHours, string? ControlForm) : IRequest<int>;

public sealed class CreateStudyPlanDisciplineHandler(IStudyPlanDisciplineRepository repository) : IRequestHandler<CreateStudyPlanDisciplineCommand, int>
{
    public async Task<int> Handle(CreateStudyPlanDisciplineCommand request, CancellationToken cancellationToken)
    {
        StudyPlanDiscipline studyPlanDiscipline = new()
        {
            StudyPlanId = request.StudyPlanId,
            FgosDisciplineId = request.FgosDisciplineId,
            SemesterNumber = request.SemesterNumber,
            PlannedHours = request.PlannedHours,
            ControlForm = request.ControlForm
        };

        await repository.AddAsync(studyPlanDiscipline, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return studyPlanDiscipline.Id;
    }
}
