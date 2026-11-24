
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.StudyGroups.Queries;

public sealed record GetStudyGroupsQuery(
    string? Search,
    int? SuborganizationId,
    int? StudyProgramId,
    int? StudyPlanId)
    : IRequest<IReadOnlyList<StudyGroupDto>>;

public sealed class GetStudyGroupsQueryHandler(
    IStudyGroupRepository repository)
    : IRequestHandler<GetStudyGroupsQuery, IReadOnlyList<StudyGroupDto>>
{
    public async Task<IReadOnlyList<StudyGroupDto>> Handle(
        GetStudyGroupsQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Entities.StudyGroup> studyGroups =
            await repository.GetAllAsync(
                request.Search,
                request.SuborganizationId,
                request.StudyProgramId,
                request.StudyPlanId,
                cancellationToken);

        return studyGroups.Select(StudyGroupDto.From).ToArray();
    }
}
