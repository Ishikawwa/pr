using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudyGroups.Commands;

public sealed record UpdateStudyGroupCommand(int Id, string Name, int SuborganizationId, int? StudyProgramId, int? StudyPlanId) : IRequest<StudyGroupDto>;

public sealed class UpdateStudyGroupCommandHandler(IStudyGroupRepository repository) : IRequestHandler<UpdateStudyGroupCommand, StudyGroupDto>
{
    public async Task<StudyGroupDto> Handle(UpdateStudyGroupCommand request, CancellationToken cancellationToken)
    {
        StudyGroup? studyGroup = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studyGroup is null)
        {
            throw new StudyGroupNotFoundException(request.Id);
        }

        string name = request.Name.Trim();

        bool nameExists = await repository.NameExistsAsync(name, request.SuborganizationId, request.Id, cancellationToken);

        if (nameExists)
        {
            throw new StudyGroupNameConflictException(name, request.SuborganizationId);
        }

        studyGroup.Name = name;
        studyGroup.SuborganizationId = request.SuborganizationId;
        studyGroup.StudyProgramId = request.StudyProgramId;
        studyGroup.StudyPlanId = request.StudyPlanId;

        await repository.UpdateAsync(studyGroup, cancellationToken);

        return StudyGroupDto.From(studyGroup);
    }
}