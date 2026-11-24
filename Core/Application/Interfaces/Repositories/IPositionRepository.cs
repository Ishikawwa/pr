using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IPositionRepository
{
    Task<IReadOnlyList<Position>> GetAllAsync(string? title, CancellationToken cancellationToken);
    Task<Position?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Position position, CancellationToken cancellationToken);
    void Update(Position position);
    void Delete(Position position);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
