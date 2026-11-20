using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IOrganizationRepository
{
    Task<IReadOnlyList<Organization>> GetAllAsync(CancellationToken cancellationToken);

    Task<Organization?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<bool> ExistsByNameAsync(string name, int? excludeId, CancellationToken cancellationToken);

    Task AddAsync(Organization organization, CancellationToken cancellationToken);

    void Update(Organization organization);

    void Delete(Organization organization);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}