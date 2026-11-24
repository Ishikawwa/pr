using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.AttestationCommissions;

public sealed record UpdateAttestationCommissionCommand(int Id, int? DisciplineId, int? StudyGroupId, int StudyProgramId, string? AcademicYear, string? SemesterLabel, int? SemesterNumber) : IRequest<bool>;

public sealed class UpdateAttestationCommissionHandler(IAttestationCommissionRepository repository) : IRequestHandler<UpdateAttestationCommissionCommand, bool>
{
    public async Task<bool> Handle(UpdateAttestationCommissionCommand request, CancellationToken cancellationToken)
    {
        AttestationCommission? commission = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (commission is null)
            return false;

        commission.DisciplineId = request.DisciplineId;
        commission.StudyGroupId = request.StudyGroupId;
        commission.StudyProgramId = request.StudyProgramId;
        commission.AcademicYear = request.AcademicYear;
        commission.SemesterLabel = request.SemesterLabel;
        commission.SemesterNumber = request.SemesterNumber;

        repository.Update(commission);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}