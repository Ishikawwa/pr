
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class StudyProgramRepository(AppDbContext context) : IStudyProgramRepository
{
    public async Task<IReadOnlyList<StudyProgram>> GetAllAsync(string? name, string? specialtyCode, int? suborganizationId, CancellationToken cancellationToken)
    {
        IQueryable<StudyProgram> query = context.StudyPrograms.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(studyProgram => EF.Functions.ILike(studyProgram.Name, $"%{name.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(specialtyCode))
            query = query.Where(studyProgram => EF.Functions.ILike(studyProgram.SpecialtyCode, $"%{specialtyCode.Trim()}%"));

        if (suborganizationId.HasValue)
            query = query.Where(studyProgram => studyProgram.SuborganizationId == suborganizationId.Value);

        return await query.OrderBy(studyProgram => studyProgram.Name).ToListAsync(cancellationToken);
    }

    public async Task<StudyProgram?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.StudyPrograms.FirstOrDefaultAsync(studyProgram => studyProgram.Id == id, cancellationToken);

    public async Task AddAsync(StudyProgram studyProgram, CancellationToken cancellationToken)
        => await context.StudyPrograms.AddAsync(studyProgram, cancellationToken);

    public void Update(StudyProgram studyProgram)
        => context.StudyPrograms.Update(studyProgram);

    public void Delete(StudyProgram studyProgram)
        => context.StudyPrograms.Remove(studyProgram);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
