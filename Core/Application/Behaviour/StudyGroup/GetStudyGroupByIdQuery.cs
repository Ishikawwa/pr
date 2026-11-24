using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudyGroups.Queries;

public sealed record GetStudyGroupByIdQuery(int Id) : IRequest<StudyGroupDto>;

public sealed class GetStudyGroupByIdQueryHandler(IStudyGroupRepository repository) : IRequestHandler<GetStudyGroupByIdQuery, StudyGroupDto>
{
    public async Task<StudyGroupDto> Handle(GetStudyGroupByIdQuery request, CancellationToken cancellationToken)
    {
        StudyGroup? studyGroup = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studyGroup is null)
        {
            throw new StudyGroupNotFoundException(request.Id);
        }

        return StudyGroupDto.From(studyGroup);
    }
}