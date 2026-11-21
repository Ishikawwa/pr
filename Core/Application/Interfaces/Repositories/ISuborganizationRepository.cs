
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ISuborganizationRepository
{
    Task<IReadOnlyList<Suborganization>> GetAllAsync(string? name, int? organizationId, CancellationToken cancellationToken);
    Task<Suborganization?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Suborganization suborganization, CancellationToken cancellationToken);
    void Update(Suborganization suborganization);
    void Delete(Suborganization suborganization);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
