using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.StudentLicenses;

public sealed record GetStudentLicenseByIdQuery(int Id) : IRequest<StudentLicense?>;

public sealed class GetStudentLicenseByIdHandler(IStudentLicenseRepository repository) : IRequestHandler<GetStudentLicenseByIdQuery, StudentLicense?>
{
    public async Task<StudentLicense?> Handle(GetStudentLicenseByIdQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.Id, cancellationToken);
}
