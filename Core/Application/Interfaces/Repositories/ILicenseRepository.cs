using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ILicenseRepository
{
    Task<IReadOnlyList<License>> GetAllAsync(string? name, CancellationToken cancellationToken);
    Task<License?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(License license, CancellationToken cancellationToken);
    void Update(License license);
    void Delete(License license);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
