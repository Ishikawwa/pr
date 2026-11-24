
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IStudyPlanRepository
{
    Task<IReadOnlyList<StudyPlan>> GetAllAsync(string? name, int? admissionBase, int? admissionYear, int? studyProgramId, CancellationToken cancellationToken);
    Task<StudyPlan?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(StudyPlan studyPlan, CancellationToken cancellationToken);
    void Update(StudyPlan studyPlan);
    void Delete(StudyPlan studyPlan);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
