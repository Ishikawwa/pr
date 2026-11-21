using Domain.Entities;

namespace Application.Features.StudyGroups;

public sealed record StudyGroupDto(
    int Id,
    string Name,
    int SuborganizationId,
    int? StudyProgramId,
    int? StudyPlanId)
{
    public static StudyGroupDto From(StudyGroup studyGroup)
    {
        return new StudyGroupDto(
            studyGroup.Id,
            studyGroup.Name,
            studyGroup.SuborganizationId,
            studyGroup.StudyProgramId,
            studyGroup.StudyPlanId
        );
    }
}