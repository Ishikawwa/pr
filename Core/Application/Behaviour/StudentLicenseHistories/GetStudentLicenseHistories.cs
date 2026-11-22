using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudentLicenseHistories;

public sealed record GetStudentLicenseHistoriesQuery(int? StudentLicenseId, int? StudentId, int? LicenseId, string? AcademicYear, int? SemesterNumber, string? SemesterLabel, string? StudentName, string? LicenseName) : IRequest<IReadOnlyList<StudentLicenseHistory>>;

public sealed class GetStudentLicenseHistoriesHandler(IStudentLicenseHistoryRepository repository) : IRequestHandler<GetStudentLicenseHistoriesQuery, IReadOnlyList<StudentLicenseHistory>>
{
    public async Task<IReadOnlyList<StudentLicenseHistory>> Handle(GetStudentLicenseHistoriesQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.StudentLicenseId, request.StudentId, request.LicenseId, request.AcademicYear, request.SemesterNumber, request.SemesterLabel, request.StudentName, request.LicenseName, cancellationToken);
}
