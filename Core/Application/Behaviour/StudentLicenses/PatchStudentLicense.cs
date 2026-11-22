using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudentLicenses;

public sealed record PatchStudentLicenseCommand(int Id, int? StudentId, int? LicenseId) : IRequest<bool>;

public sealed class PatchStudentLicenseHandler(IStudentLicenseRepository repository) : IRequestHandler<PatchStudentLicenseCommand, bool>
{
    public async Task<bool> Handle(PatchStudentLicenseCommand request, CancellationToken cancellationToken)
    {
        StudentLicense? studentLicense = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studentLicense is null)
            return false;

        if (request.StudentId.HasValue)
            studentLicense.StudentId = request.StudentId.Value;

        if (request.LicenseId.HasValue)
            studentLicense.LicenseId = request.LicenseId.Value;

        repository.Update(studentLicense);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
