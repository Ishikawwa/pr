using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudyGroups.Queries;

public sealed record GetStudyGroupsQuery : IRequest<IReadOnlyList<StudyGroupDto>>;

public sealed class GetStudyGroupsQueryHandler(IStudyGroupRepository repository) : IRequestHandler<GetStudyGroupsQuery, IReadOnlyList<StudyGroupDto>>
{
    public async Task<IReadOnlyList<StudyGroupDto>> Handle(GetStudyGroupsQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<StudyGroup> studyGroups = await repository.GetAllAsync(cancellationToken);

        return studyGroups.Select(StudyGroupDto.From).ToArray();
    }
}