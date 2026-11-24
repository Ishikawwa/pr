
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IStudyGroupRepository
{
    Task<IReadOnlyList<StudyGroup>> GetAllAsync(
        string? search,
        int? suborganizationId,
        int? studyProgramId,
        int? studyPlanId,
        CancellationToken cancellationToken);

    Task<StudyGroup?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<bool> NameExistsAsync(
        string name,
        int suborganizationId,
        int? excludingId,
        CancellationToken cancellationToken);

    Task<StudyGroup> AddAsync(StudyGroup studyGroup, CancellationToken cancellationToken);

    Task UpdateAsync(StudyGroup studyGroup, CancellationToken cancellationToken);

    Task DeleteAsync(StudyGroup studyGroup, CancellationToken cancellationToken);
}
