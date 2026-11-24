using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPlans;

public sealed record UpdateStudyPlanCommand(int Id, int AdmissionBase, int AdmissionYear, string? Name, string? SourceFileName, int StudyProgramId) : IRequest<bool>;

public sealed class UpdateStudyPlanHandler(IStudyPlanRepository repository) : IRequestHandler<UpdateStudyPlanCommand, bool>
{
    public async Task<bool> Handle(UpdateStudyPlanCommand request, CancellationToken cancellationToken)
    {
        StudyPlan? studyPlan = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studyPlan is null)
            return false;

        studyPlan.AdmissionBase = request.AdmissionBase;
        studyPlan.AdmissionYear = request.AdmissionYear;
        studyPlan.Name = request.Name;
        studyPlan.SourceFileName = request.SourceFileName;
        studyPlan.StudyProgramId = request.StudyProgramId;

        repository.Update(studyPlan);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
