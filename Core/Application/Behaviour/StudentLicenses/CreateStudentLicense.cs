using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudentLicenses;

public sealed record CreateStudentLicenseCommand(int StudentId, int LicenseId) : IRequest<int>;

public sealed class CreateStudentLicenseHandler(IStudentLicenseRepository repository) : IRequestHandler<CreateStudentLicenseCommand, int>
{
    public async Task<int> Handle(CreateStudentLicenseCommand request, CancellationToken cancellationToken)
    {
        StudentLicense studentLicense = new()
        {
            StudentId = request.StudentId,
            LicenseId = request.LicenseId
        };

        await repository.AddAsync(studentLicense, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return studentLicense.Id;
    }
}
