using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.AttestationCommissions;

public sealed record PatchAttestationCommissionCommand(int Id, int? DisciplineId, int? StudyGroupId, int? StudyProgramId, string? AcademicYear, string? SemesterLabel, int? SemesterNumber) : IRequest<bool>;

public sealed class PatchAttestationCommissionHandler(IAttestationCommissionRepository repository) : IRequestHandler<PatchAttestationCommissionCommand, bool>
{
    public async Task<bool> Handle(PatchAttestationCommissionCommand request, CancellationToken cancellationToken)
    {
        AttestationCommission? commission = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (commission is null)
            return false;

        if (request.DisciplineId.HasValue)
            commission.DisciplineId = request.DisciplineId.Value;

        if (request.StudyGroupId.HasValue)
            commission.StudyGroupId = request.StudyGroupId.Value;

        if (request.StudyProgramId.HasValue)
            commission.StudyProgramId = request.StudyProgramId.Value;

        if (request.AcademicYear is not null)
            commission.AcademicYear = request.AcademicYear;

        if (request.SemesterLabel is not null)
            commission.SemesterLabel = request.SemesterLabel;

        if (request.SemesterNumber.HasValue)
            commission.SemesterNumber = request.SemesterNumber.Value;

        repository.Update(commission);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}