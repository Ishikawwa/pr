
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class PositionRepository(AppDbContext context) : IPositionRepository
{
    public async Task<IReadOnlyList<Position>> GetAllAsync(string? title, CancellationToken cancellationToken)
    {
        IQueryable<Position> query = context.Positions.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(position => EF.Functions.ILike(position.Title, $"%{title.Trim()}%"));

        return await query.OrderBy(position => position.Title).ToListAsync(cancellationToken);
    }

    public async Task<Position?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.Positions.FirstOrDefaultAsync(position => position.Id == id, cancellationToken);

    public async Task AddAsync(Position position, CancellationToken cancellationToken)
        => await context.Positions.AddAsync(position, cancellationToken);

    public void Update(Position position)
        => context.Positions.Update(position);

    public void Delete(Position position)
        => context.Positions.Remove(position);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
