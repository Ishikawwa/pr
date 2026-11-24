using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IStudyPlanDisciplineRepository
{
    Task<IReadOnlyList<StudyPlanDiscipline>> GetAllAsync(int? studyPlanId, int? fgosDisciplineId, int? semesterNumber, string? controlForm, string? fgosDisciplineCode, string? fgosDisciplineName, CancellationToken cancellationToken);
    Task<StudyPlanDiscipline?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(StudyPlanDiscipline studyPlanDiscipline, CancellationToken cancellationToken);
    void Update(StudyPlanDiscipline studyPlanDiscipline);
    void Delete(StudyPlanDiscipline studyPlanDiscipline);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
