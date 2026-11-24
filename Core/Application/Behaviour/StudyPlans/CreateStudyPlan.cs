using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPlans;

public sealed record CreateStudyPlanCommand(int AdmissionBase, int AdmissionYear, string? Name, string? SourceFileName, int StudyProgramId) : IRequest<int>;

public sealed class CreateStudyPlanHandler(IStudyPlanRepository repository) : IRequestHandler<CreateStudyPlanCommand, int>
{
    public async Task<int> Handle(CreateStudyPlanCommand request, CancellationToken cancellationToken)
    {
        StudyPlan studyPlan = new()
        {
            AdmissionBase = request.AdmissionBase,
            AdmissionYear = request.AdmissionYear,
            Name = request.Name,
            SourceFileName = request.SourceFileName,
            StudyProgramId = request.StudyProgramId
        };

        await repository.AddAsync(studyPlan, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return studyPlan.Id;
    }
}
