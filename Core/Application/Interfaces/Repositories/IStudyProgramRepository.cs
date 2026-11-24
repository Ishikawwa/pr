
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IStudyProgramRepository
{
    Task<IReadOnlyList<StudyProgram>> GetAllAsync(string? name, string? specialtyCode, int? suborganizationId, CancellationToken cancellationToken);
    Task<StudyProgram?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(StudyProgram studyProgram, CancellationToken cancellationToken);
    void Update(StudyProgram studyProgram);
    void Delete(StudyProgram studyProgram);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
