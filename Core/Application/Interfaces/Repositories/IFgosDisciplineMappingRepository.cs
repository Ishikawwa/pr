using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IFgosDisciplineMappingRepository
{
    Task<IReadOnlyList<FgosDisciplineMapping>> GetAllAsync(int? disciplineId, int? fgosDisciplineId, string? disciplineCode, string? disciplineName, string? fgosDisciplineCode, string? fgosDisciplineName, CancellationToken cancellationToken);
    Task<FgosDisciplineMapping?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(FgosDisciplineMapping mapping, CancellationToken cancellationToken);
    void Update(FgosDisciplineMapping mapping);
    void Delete(FgosDisciplineMapping mapping);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
