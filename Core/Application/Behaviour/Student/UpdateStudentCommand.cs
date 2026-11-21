using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.Students.Commands;

public sealed record UpdateStudentCommand(int Id, string Email, string FirstName, string LastName, string? Patronymic, int? AdmissionBase, int? AdmissionYear) : IRequest<StudentDto>;

public sealed class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(320);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Patronymic)
            .MaximumLength(200);

        RuleFor(x => x.AdmissionYear)
            .InclusiveBetween(1900, 2200)
            .When(x => x.AdmissionYear.HasValue);
    }
}

public sealed class UpdateStudentCommandHandler(IStudentRepository repository, IValidator<UpdateStudentCommand> validator) : IRequestHandler<UpdateStudentCommand, StudentDto>
{
    public async Task<StudentDto> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        Student student = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (student is null)
        {
            throw new StudentNotFoundException(request.Id);
        }

        string email = request.Email.Trim();

        bool emailExists = await repository.EmailExistsAsync(email, request.Id, cancellationToken);

        if (emailExists)
        {
            throw new StudentEmailConflictException(email);
        }

        student.Email = email;
        student.FirstName = request.FirstName.Trim();
        student.LastName = request.LastName.Trim();
        student.Patronymic = request.Patronymic?.Trim();
        student.AdmissionBase = request.AdmissionBase;
        student.AdmissionYear = request.AdmissionYear;

        await repository.UpdateAsync(student, cancellationToken);

        return StudentDto.From(student);
    }
}