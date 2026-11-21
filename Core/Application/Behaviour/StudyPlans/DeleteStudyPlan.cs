using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPlans;

public sealed record DeleteStudyPlanCommand(int Id) : IRequest<bool>;

public sealed class DeleteStudyPlanHandler(IStudyPlanRepository repository) : IRequestHandler<DeleteStudyPlanCommand, bool>
{
    public async Task<bool> Handle(DeleteStudyPlanCommand request, CancellationToken cancellationToken)
    {
        StudyPlan? studyPlan = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studyPlan is null)
            return false;

        repository.Delete(studyPlan);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
