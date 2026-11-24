using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPlanDisciplines;

public sealed record DeleteStudyPlanDisciplineCommand(int Id) : IRequest<bool>;

public sealed class DeleteStudyPlanDisciplineHandler(IStudyPlanDisciplineRepository repository) : IRequestHandler<DeleteStudyPlanDisciplineCommand, bool>
{
    public async Task<bool> Handle(DeleteStudyPlanDisciplineCommand request, CancellationToken cancellationToken)
    {
        StudyPlanDiscipline? studyPlanDiscipline = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studyPlanDiscipline is null)
            return false;

        repository.Delete(studyPlanDiscipline);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
