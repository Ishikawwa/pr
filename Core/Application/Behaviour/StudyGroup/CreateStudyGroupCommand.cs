using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudyGroups.Commands;

public sealed record CreateStudyGroupCommand(string Name, int SuborganizationId, int? StudyProgramId, int? StudyPlanId) : IRequest<StudyGroupDto>;

public sealed class CreateStudyGroupCommandHandler(IStudyGroupRepository repository) : IRequestHandler<CreateStudyGroupCommand, StudyGroupDto>
{
    public async Task<StudyGroupDto> Handle(CreateStudyGroupCommand request, CancellationToken cancellationToken)
    {
        string name = request.Name.Trim();

        bool nameExists = await repository.NameExistsAsync(name, request.SuborganizationId, null, cancellationToken);

        if (nameExists)
        {
            throw new StudyGroupNameConflictException(name, request.SuborganizationId);
        }

        StudyGroup studyGroup = new StudyGroup
        {
            Name = name,
            SuborganizationId = request.SuborganizationId,
            StudyProgramId = request.StudyProgramId,
            StudyPlanId = request.StudyPlanId
        };

        StudyGroup createdStudyGroup = await repository.AddAsync(studyGroup, cancellationToken);

        return StudyGroupDto.From(createdStudyGroup);
    }
}