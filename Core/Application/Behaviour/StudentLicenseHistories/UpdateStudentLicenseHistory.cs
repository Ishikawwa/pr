using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudentLicenseHistories;

public sealed record UpdateStudentLicenseHistoryCommand(int Id, int StudentLicenseId, string? AcademicYear, int? SemesterNumber, string SemesterLabel) : IRequest<bool>;

public sealed class UpdateStudentLicenseHistoryHandler(IStudentLicenseHistoryRepository repository) : IRequestHandler<UpdateStudentLicenseHistoryCommand, bool>
{
    public async Task<bool> Handle(UpdateStudentLicenseHistoryCommand request, CancellationToken cancellationToken)
    {
        StudentLicenseHistory? studentLicenseHistory = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studentLicenseHistory is null)
            return false;

        studentLicenseHistory.StudentLicenseId = request.StudentLicenseId;
        studentLicenseHistory.AcademicYear = request.AcademicYear;
        studentLicenseHistory.SemesterNumber = request.SemesterNumber;
        studentLicenseHistory.SemesterLabel = request.SemesterLabel;

        repository.Update(studentLicenseHistory);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
