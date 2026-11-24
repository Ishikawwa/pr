using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IFgosDisciplineRepository
{
    Task<IReadOnlyList<FgosDiscipline>> GetAllAsync(string? name, string? code, int? studyProgramId, CancellationToken cancellationToken);
    Task<FgosDiscipline?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(FgosDiscipline fgosDiscipline, CancellationToken cancellationToken);
    void Update(FgosDiscipline fgosDiscipline);
    void Delete(FgosDiscipline fgosDiscipline);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
