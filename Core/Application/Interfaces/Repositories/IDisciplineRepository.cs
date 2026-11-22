using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IDisciplineRepository
{
    Task<IReadOnlyList<Discipline>> GetAllAsync(string? code, string? name, int? suborganizationId, CancellationToken cancellationToken);
    Task<Discipline?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Discipline discipline, CancellationToken cancellationToken);
    void Update(Discipline discipline);
    void Delete(Discipline discipline);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
