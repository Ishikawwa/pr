using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudentLicenses;

public sealed record DeleteStudentLicenseCommand(int Id) : IRequest<bool>;

public sealed class DeleteStudentLicenseHandler(IStudentLicenseRepository repository) : IRequestHandler<DeleteStudentLicenseCommand, bool>
{
    public async Task<bool> Handle(DeleteStudentLicenseCommand request, CancellationToken cancellationToken)
    {
        StudentLicense? studentLicense = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studentLicense is null)
            return false;

        repository.Delete(studentLicense);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
