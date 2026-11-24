
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class StudyGroupRepository(AppDbContext db) : IStudyGroupRepository
{
    public async Task<IReadOnlyList<StudyGroup>> GetAllAsync(
        string? search,
        int? suborganizationId,
        int? studyProgramId,
        int? studyPlanId,
        CancellationToken cancellationToken)
    {
        IQueryable<StudyGroup> query = db.StudyGroups.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                EF.Functions.ILike(x.Name, $"%{search.Trim()}%"));
        }

        if (suborganizationId.HasValue)
        {
            query = query.Where(x =>
                x.SuborganizationId == suborganizationId.Value);
        }

        if (studyProgramId.HasValue)
        {
            query = query.Where(x =>
                x.StudyProgramId == studyProgramId.Value);
        }

        if (studyPlanId.HasValue)
        {
            query = query.Where(x =>
                x.StudyPlanId == studyPlanId.Value);
        }

        return await query
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<StudyGroup?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await db.StudyGroups
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> NameExistsAsync(
        string name,
        int suborganizationId,
        int? excludingId,
        CancellationToken cancellationToken)
    {
        return await db.StudyGroups.AnyAsync(
            x => x.Name == name &&
                 x.SuborganizationId == suborganizationId &&
                 (!excludingId.HasValue || x.Id != excludingId.Value),
            cancellationToken);
    }

    public async Task<StudyGroup> AddAsync(
        StudyGroup studyGroup,
        CancellationToken cancellationToken)
    {
        db.StudyGroups.Add(studyGroup);

        await db.SaveChangesAsync(cancellationToken);

        return studyGroup;
    }

    public async Task UpdateAsync(
        StudyGroup studyGroup,
        CancellationToken cancellationToken)
    {
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        StudyGroup studyGroup,
        CancellationToken cancellationToken)
    {
        db.StudyGroups.Remove(studyGroup);

        await db.SaveChangesAsync(cancellationToken);
    }
}
