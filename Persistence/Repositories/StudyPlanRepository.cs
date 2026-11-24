
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class StudyPlanRepository(AppDbContext context) : IStudyPlanRepository
{
    public async Task<IReadOnlyList<StudyPlan>> GetAllAsync(string? name, int? admissionBase, int? admissionYear, int? studyProgramId, CancellationToken cancellationToken)
    {
        IQueryable<StudyPlan> query = context.StudyPlans.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(studyPlan => studyPlan.Name != null && EF.Functions.ILike(studyPlan.Name, $"%{name}%"));

        if (admissionBase.HasValue)
            query = query.Where(studyPlan => studyPlan.AdmissionBase == admissionBase.Value);

        if (admissionYear.HasValue)
            query = query.Where(studyPlan => studyPlan.AdmissionYear == admissionYear.Value);

        if (studyProgramId.HasValue)
            query = query.Where(studyPlan => studyPlan.StudyProgramId == studyProgramId.Value);

        return await query.OrderBy(studyPlan => studyPlan.Name).ThenBy(studyPlan => studyPlan.AdmissionYear).ToListAsync(cancellationToken);
    }

    public async Task<StudyPlan?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.StudyPlans.FirstOrDefaultAsync(studyPlan => studyPlan.Id == id, cancellationToken);

    public async Task AddAsync(StudyPlan studyPlan, CancellationToken cancellationToken)
        => await context.StudyPlans.AddAsync(studyPlan, cancellationToken);

    public void Update(StudyPlan studyPlan)
        => context.StudyPlans.Update(studyPlan);

    public void Delete(StudyPlan studyPlan)
        => context.StudyPlans.Remove(studyPlan);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
