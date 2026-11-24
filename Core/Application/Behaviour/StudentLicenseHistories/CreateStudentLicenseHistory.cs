using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudentLicenseHistories;

public sealed record CreateStudentLicenseHistoryCommand(int StudentLicenseId, string? AcademicYear, int? SemesterNumber, string SemesterLabel) : IRequest<int>;

public sealed class CreateStudentLicenseHistoryHandler(IStudentLicenseHistoryRepository repository) : IRequestHandler<CreateStudentLicenseHistoryCommand, int>
{
    public async Task<int> Handle(CreateStudentLicenseHistoryCommand request, CancellationToken cancellationToken)
    {
        StudentLicenseHistory studentLicenseHistory = new()
        {
            StudentLicenseId = request.StudentLicenseId,
            AcademicYear = request.AcademicYear,
            SemesterNumber = request.SemesterNumber,
            SemesterLabel = request.SemesterLabel
        };

        await repository.AddAsync(studentLicenseHistory, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return studentLicenseHistory.Id;
    }
}
