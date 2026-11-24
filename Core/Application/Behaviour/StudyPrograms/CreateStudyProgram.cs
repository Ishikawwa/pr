using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPrograms;

public sealed record CreateStudyProgramCommand(string Name, string Qualification, string SpecialtyCode, int SuborganizationId) : IRequest<int>;

public sealed class CreateStudyProgramHandler(IStudyProgramRepository repository) : IRequestHandler<CreateStudyProgramCommand, int>
{
    public async Task<int> Handle(CreateStudyProgramCommand request, CancellationToken cancellationToken)
    {
        StudyProgram studyProgram = new()
        {
            Name = request.Name,
            Qualification = request.Qualification,
            SpecialtyCode = request.SpecialtyCode,
            SuborganizationId = request.SuborganizationId
        };

        await repository.AddAsync(studyProgram, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return studyProgram.Id;
    }
}
