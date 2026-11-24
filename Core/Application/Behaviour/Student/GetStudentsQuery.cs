
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Students.Queries;

public sealed record GetStudentsQuery(string? Search, int? AdmissionBase, int? AdmissionYear) : IRequest<IReadOnlyList<StudentDto>>;

public sealed class GetStudentsQueryHandler(IStudentRepository repository) : IRequestHandler<GetStudentsQuery, IReadOnlyList<StudentDto>>
{
    public async Task<IReadOnlyList<StudentDto>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<Student> students = await repository.GetAllAsync(
            request.Search,
            request.AdmissionBase,
            request.AdmissionYear,
            cancellationToken);

        return students.Select(StudentDto.From).ToArray();
    }
}
