using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class AttestationCommissionRepository(AppDbContext context) : IAttestationCommissionRepository
{
    public async Task<IReadOnlyList<AttestationCommission>> GetAllAsync(int? disciplineId, int? studyGroupId, int? studyProgramId, string? academicYear, string? semesterLabel, int? semesterNumber, string? disciplineName, string? studyGroupName, CancellationToken cancellationToken)
    {
        IQueryable<AttestationCommission> query = context.AttestationCommissions
            .AsNoTracking()
            .Include(commission => commission.Discipline)
            .Include(commission => commission.StudyGroup)
            .Include(commission => commission.StudyProgram);

        if (disciplineId.HasValue)
            query = query.Where(commission => commission.DisciplineId == disciplineId.Value);

        if (studyGroupId.HasValue)
            query = query.Where(commission => commission.StudyGroupId == studyGroupId.Value);

        if (studyProgramId.HasValue)
            query = query.Where(commission => commission.StudyProgramId == studyProgramId.Value);

        if (!string.IsNullOrWhiteSpace(academicYear))
            query = query.Where(commission => commission.AcademicYear != null && EF.Functions.ILike(commission.AcademicYear, $"%{academicYear.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(semesterLabel))
            query = query.Where(commission => commission.SemesterLabel != null && EF.Functions.ILike(commission.SemesterLabel, $"%{semesterLabel.Trim()}%"));

        if (semesterNumber.HasValue)
            query = query.Where(commission => commission.SemesterNumber == semesterNumber.Value);

        if (!string.IsNullOrWhiteSpace(disciplineName))
            query = query.Where(commission => commission.Discipline != null && EF.Functions.ILike(commission.Discipline.Name, $"%{disciplineName.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(studyGroupName))
            query = query.Where(commission => commission.StudyGroup != null && EF.Functions.ILike(commission.StudyGroup.Name, $"%{studyGroupName.Trim()}%"));

        return await query.OrderBy(commission => commission.StudyProgramId)
            .ThenBy(commission => commission.AcademicYear)
            .ThenBy(commission => commission.SemesterNumber)
            .ThenBy(commission => commission.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<AttestationCommission?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.AttestationCommissions
            .Include(commission => commission.Discipline)
            .Include(commission => commission.StudyGroup)
            .Include(commission => commission.StudyProgram)
            .FirstOrDefaultAsync(commission => commission.Id == id, cancellationToken);

    public async Task AddAsync(AttestationCommission commission, CancellationToken cancellationToken)
        => await context.AttestationCommissions.AddAsync(commission, cancellationToken);

    public void Update(AttestationCommission commission)
        => context.AttestationCommissions.Update(commission);

    public void Delete(AttestationCommission commission)
        => context.AttestationCommissions.Remove(commission);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);
}