using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Students.Queries;

public sealed record GetStudentByIdQuery(int Id) : IRequest<StudentDto>;

public sealed class GetStudentByIdQueryHandler(IStudentRepository repository) : IRequestHandler<GetStudentByIdQuery, StudentDto>
{
    public async Task<StudentDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        Student student = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (student is null)
        {
            throw new StudentNotFoundException(request.Id);
        }

        return StudentDto.From(student);
    }
}