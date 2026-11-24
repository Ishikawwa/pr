using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.AttestationCommissions;

public sealed record GetAttestationCommissionsQuery(int? DisciplineId, int? StudyGroupId, int? StudyProgramId, string? AcademicYear, string? SemesterLabel, int? SemesterNumber, string? DisciplineName, string? StudyGroupName) : IRequest<IReadOnlyList<AttestationCommission>>;

public sealed class GetAttestationCommissionsHandler(IAttestationCommissionRepository repository) : IRequestHandler<GetAttestationCommissionsQuery, IReadOnlyList<AttestationCommission>>
{
    public async Task<IReadOnlyList<AttestationCommission>> Handle(GetAttestationCommissionsQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.DisciplineId, request.StudyGroupId, request.StudyProgramId, request.AcademicYear, request.SemesterLabel, request.SemesterNumber, request.DisciplineName, request.StudyGroupName, cancellationToken);
}