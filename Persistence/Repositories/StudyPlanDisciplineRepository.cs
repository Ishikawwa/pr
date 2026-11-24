using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class StudyPlanDisciplineRepository(AppDbContext context) : IStudyPlanDisciplineRepository
{
    public async Task<IReadOnlyList<StudyPlanDiscipline>> GetAllAsync(int? studyPlanId, int? fgosDisciplineId, int? semesterNumber, string? controlForm, string? fgosDisciplineCode, string? fgosDisciplineName, CancellationToken cancellationToken)
    {
        IQueryable<StudyPlanDiscipline> query = context.StudyPlanDisciplines
            .AsNoTracking()
            .Include(studyPlanDiscipline => studyPlanDiscipline.StudyPlan)
            .Include(studyPlanDiscipline => studyPlanDiscipline.FgosDiscipline);

        if (studyPlanId.HasValue)
            query = query.Where(studyPlanDiscipline => studyPlanDiscipline.StudyPlanId == studyPlanId.Value);

        if (fgosDisciplineId.HasValue)
            query = query.Where(studyPlanDiscipline => studyPlanDiscipline.FgosDisciplineId == fgosDisciplineId.Value);

        if (semesterNumber.HasValue)
            query = query.Where(studyPlanDiscipline => studyPlanDiscipline.SemesterNumber == semesterNumber.Value);

        if (!string.IsNullOrWhiteSpace(controlForm))
            query = query.Where(studyPlanDiscipline => studyPlanDiscipline.ControlForm != null && EF.Functions.ILike(studyPlanDiscipline.ControlForm, $"%{controlForm.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(fgosDisciplineCode))
            query = query.Where(studyPlanDiscipline => EF.Functions.ILike(studyPlanDiscipline.FgosDiscipline.Code, $"%{fgosDisciplineCode.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(fgosDisciplineName))
            query = query.Where(studyPlanDiscipline => EF.Functions.ILike(studyPlanDiscipline.FgosDiscipline.Name, $"%{fgosDisciplineName.Trim()}%"));

        return await query.OrderBy(studyPlanDiscipline => studyPlanDiscipline.StudyPlanId)
            .ThenBy(studyPlanDiscipline => studyPlanDiscipline.SemesterNumber)
            .ThenBy(studyPlanDiscipline => studyPlanDiscipline.FgosDiscipline.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<StudyPlanDiscipline?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.StudyPlanDisciplines
            .Include(studyPlanDiscipline => studyPlanDiscipline.StudyPlan)
            .Include(studyPlanDiscipline => studyPlanDiscipline.FgosDiscipline)
            .FirstOrDefaultAsync(studyPlanDiscipline => studyPlanDiscipline.Id == id, cancellationToken);

    public async Task AddAsync(StudyPlanDiscipline studyPlanDiscipline, CancellationToken cancellationToken)
        => await context.StudyPlanDisciplines.AddAsync(studyPlanDiscipline, cancellationToken);

    public void Update(StudyPlanDiscipline studyPlanDiscipline)
        => context.StudyPlanDisciplines.Update(studyPlanDiscipline);

    public void Delete(StudyPlanDiscipline studyPlanDiscipline)
        => context.StudyPlanDisciplines.Remove(studyPlanDiscipline);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}
