using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.AttestationCommissions;

public sealed record CreateAttestationCommissionCommand(int? DisciplineId, int? StudyGroupId, int StudyProgramId, string? AcademicYear, string? SemesterLabel, int? SemesterNumber) : IRequest<int>;

public sealed class CreateAttestationCommissionHandler(IAttestationCommissionRepository repository) : IRequestHandler<CreateAttestationCommissionCommand, int>
{
    public async Task<int> Handle(CreateAttestationCommissionCommand request, CancellationToken cancellationToken)
    {
        AttestationCommission commission = new()
        {
            DisciplineId = request.DisciplineId,
            StudyGroupId = request.StudyGroupId,
            StudyProgramId = request.StudyProgramId,
            AcademicYear = request.AcademicYear,
            SemesterLabel = request.SemesterLabel,
            SemesterNumber = request.SemesterNumber
        };

        await repository.AddAsync(commission, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return commission.Id;
    }
}