using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudentLicenseHistories;

public sealed record PatchStudentLicenseHistoryCommand(int Id, int? StudentLicenseId, string? AcademicYear, int? SemesterNumber, string? SemesterLabel) : IRequest<bool>;

public sealed class PatchStudentLicenseHistoryHandler(IStudentLicenseHistoryRepository repository) : IRequestHandler<PatchStudentLicenseHistoryCommand, bool>
{
    public async Task<bool> Handle(PatchStudentLicenseHistoryCommand request, CancellationToken cancellationToken)
    {
        StudentLicenseHistory? studentLicenseHistory = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studentLicenseHistory is null)
            return false;

        if (request.StudentLicenseId.HasValue)
            studentLicenseHistory.StudentLicenseId = request.StudentLicenseId.Value;

        if (request.AcademicYear is not null)
            studentLicenseHistory.AcademicYear = request.AcademicYear;

        if (request.SemesterNumber.HasValue)
            studentLicenseHistory.SemesterNumber = request.SemesterNumber.Value;

        if (request.SemesterLabel is not null)
            studentLicenseHistory.SemesterLabel = request.SemesterLabel;

        repository.Update(studentLicenseHistory);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
