using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Students.Queries;

public sealed record GetStudentsQuery
    : IRequest<IReadOnlyList<StudentDto>>;

public sealed class GetStudentsQueryHandler(
    IStudentRepository repository)
    : IRequestHandler<GetStudentsQuery, IReadOnlyList<StudentDto>>
{
    public async Task<IReadOnlyList<StudentDto>> Handle(
        GetStudentsQuery request,
        CancellationToken cancellationToken)
    {
        var students = await repository.GetAllAsync(
            cancellationToken);

        return students
            .Select(StudentDto.From)
            .ToArray();
    }
}