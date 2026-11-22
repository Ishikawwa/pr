using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudentLicenses;

public sealed record GetStudentLicensesQuery(int? StudentId, int? LicenseId, string? StudentName, string? LicenseName) : IRequest<IReadOnlyList<StudentLicense>>;

public sealed class GetStudentLicensesHandler(IStudentLicenseRepository repository) : IRequestHandler<GetStudentLicensesQuery, IReadOnlyList<StudentLicense>>
{
    public async Task<IReadOnlyList<StudentLicense>> Handle(GetStudentLicensesQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.StudentId, request.LicenseId, request.StudentName, request.LicenseName, cancellationToken);
}
