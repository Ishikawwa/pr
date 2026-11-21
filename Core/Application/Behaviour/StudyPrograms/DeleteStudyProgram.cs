using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPrograms;

public sealed record DeleteStudyProgramCommand(int Id) : IRequest<bool>;

public sealed class DeleteStudyProgramHandler(IStudyProgramRepository repository) : IRequestHandler<DeleteStudyProgramCommand, bool>
{
    public async Task<bool> Handle(DeleteStudyProgramCommand request, CancellationToken cancellationToken)
    {
        StudyProgram? studyProgram = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studyProgram is null)
            return false;

        repository.Delete(studyProgram);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
