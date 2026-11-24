using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudentLicenses;

public sealed record UpdateStudentLicenseCommand(int Id, int StudentId, int LicenseId) : IRequest<bool>;

public sealed class UpdateStudentLicenseHandler(IStudentLicenseRepository repository) : IRequestHandler<UpdateStudentLicenseCommand, bool>
{
    public async Task<bool> Handle(UpdateStudentLicenseCommand request, CancellationToken cancellationToken)
    {
        StudentLicense? studentLicense = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studentLicense is null)
            return false;

        studentLicense.StudentId = request.StudentId;
        studentLicense.LicenseId = request.LicenseId;

        repository.Update(studentLicense);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
