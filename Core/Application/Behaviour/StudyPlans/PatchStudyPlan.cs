using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPlans;

public sealed record PatchStudyPlanCommand(int Id, int? AdmissionBase, int? AdmissionYear, string? Name, string? SourceFileName, int? StudyProgramId) : IRequest<bool>;

public sealed class PatchStudyPlanHandler(IStudyPlanRepository repository) : IRequestHandler<PatchStudyPlanCommand, bool>
{
    public async Task<bool> Handle(PatchStudyPlanCommand request, CancellationToken cancellationToken)
    {
        StudyPlan? studyPlan = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studyPlan is null)
            return false;

        if (request.AdmissionBase.HasValue)
            studyPlan.AdmissionBase = request.AdmissionBase.Value;

        if (request.AdmissionYear.HasValue)
            studyPlan.AdmissionYear = request.AdmissionYear.Value;

        if (request.Name is not null)
            studyPlan.Name = request.Name;

        if (request.SourceFileName is not null)
            studyPlan.SourceFileName = request.SourceFileName;

        if (request.StudyProgramId.HasValue)
            studyPlan.StudyProgramId = request.StudyProgramId.Value;

        repository.Update(studyPlan);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
