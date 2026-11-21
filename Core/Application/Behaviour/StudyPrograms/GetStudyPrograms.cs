
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudyPrograms;

public sealed record GetStudyProgramsQuery(string? Name, string? SpecialtyCode, int? SuborganizationId) : IRequest<IReadOnlyList<StudyProgram>>;

public sealed class GetStudyProgramsHandler(IStudyProgramRepository repository) : IRequestHandler<GetStudyProgramsQuery, IReadOnlyList<StudyProgram>>
{
    public async Task<IReadOnlyList<StudyProgram>> Handle(GetStudyProgramsQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.Name, request.SpecialtyCode, request.SuborganizationId, cancellationToken);
}
