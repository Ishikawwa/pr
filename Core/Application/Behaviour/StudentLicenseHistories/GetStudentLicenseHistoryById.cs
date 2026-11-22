using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudentLicenseHistories;

public sealed record GetStudentLicenseHistoryByIdQuery(int Id) : IRequest<StudentLicenseHistory?>;

public sealed class GetStudentLicenseHistoryByIdHandler(IStudentLicenseHistoryRepository repository) : IRequestHandler<GetStudentLicenseHistoryByIdQuery, StudentLicenseHistory?>
{
    public async Task<StudentLicenseHistory?> Handle(GetStudentLicenseHistoryByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}
