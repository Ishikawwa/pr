using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudentLicenseHistories;

public sealed record DeleteStudentLicenseHistoryCommand(int Id) : IRequest<bool>;

public sealed class DeleteStudentLicenseHistoryHandler(IStudentLicenseHistoryRepository repository) : IRequestHandler<DeleteStudentLicenseHistoryCommand, bool>
{
    public async Task<bool> Handle(DeleteStudentLicenseHistoryCommand request, CancellationToken cancellationToken)
    {
        StudentLicenseHistory? studentLicenseHistory = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studentLicenseHistory is null)
            return false;

        repository.Delete(studentLicenseHistory);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
