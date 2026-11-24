using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPrograms;

public sealed record PatchStudyProgramCommand(int Id, string? Name, string? Qualification, string? SpecialtyCode, int? SuborganizationId) : IRequest<bool>;

public sealed class PatchStudyProgramHandler(IStudyProgramRepository repository) : IRequestHandler<PatchStudyProgramCommand, bool>
{
    public async Task<bool> Handle(PatchStudyProgramCommand request, CancellationToken cancellationToken)
    {
        StudyProgram? studyProgram = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studyProgram is null)
            return false;

        if (request.Name is not null)
            studyProgram.Name = request.Name;

        if (request.Qualification is not null)
            studyProgram.Qualification = request.Qualification;

        if (request.SpecialtyCode is not null)
            studyProgram.SpecialtyCode = request.SpecialtyCode;

        if (request.SuborganizationId.HasValue)
            studyProgram.SuborganizationId = request.SuborganizationId.Value;

        repository.Update(studyProgram);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
