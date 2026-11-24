using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPrograms;

public sealed record UpdateStudyProgramCommand(int Id, string Name, string Qualification, string SpecialtyCode, int SuborganizationId) : IRequest<bool>;

public sealed class UpdateStudyProgramHandler(IStudyProgramRepository repository) : IRequestHandler<UpdateStudyProgramCommand, bool>
{
    public async Task<bool> Handle(UpdateStudyProgramCommand request, CancellationToken cancellationToken)
    {
        StudyProgram? studyProgram = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studyProgram is null)
            return false;

        studyProgram.Name = request.Name;
        studyProgram.Qualification = request.Qualification;
        studyProgram.SpecialtyCode = request.SpecialtyCode;
        studyProgram.SuborganizationId = request.SuborganizationId;

        repository.Update(studyProgram);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
