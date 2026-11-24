using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPrograms;

public sealed record GetStudyProgramByIdQuery(int Id) : IRequest<StudyProgram?>;

public sealed class GetStudyProgramByIdHandler(IStudyProgramRepository repository) : IRequestHandler<GetStudyProgramByIdQuery, StudyProgram?>
{
    public async Task<StudyProgram?> Handle(GetStudyProgramByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}
