using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.Students.Commands;

public sealed record CreateStudentCommand(
    string Email,
    string FirstName,
    string LastName,
    string? Patronymic,
    int? AdmissionBase,
    int? AdmissionYear
) : IRequest<StudentDto>;

public sealed class CreateStudentCommandValidator
    : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
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

public sealed class CreateStudentCommandHandler(
    IStudentRepository repository,
    IValidator<CreateStudentCommand> validator)
    : IRequestHandler<CreateStudentCommand, StudentDto>
{
    public async Task<StudentDto> Handle(
        CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var email = request.Email.Trim();

        var emailExists = await repository.EmailExistsAsync(
            email,
            null,
            cancellationToken);

        if (emailExists)
        {
            throw new StudentEmailConflictException(email);
        }

        var student = new Student
        {
            Email = email,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Patronymic = request.Patronymic?.Trim(),
            AdmissionBase = request.AdmissionBase,
            AdmissionYear = request.AdmissionYear
        };

        var createdStudent = await repository.AddAsync(
            student,
            cancellationToken);

        return StudentDto.From(createdStudent);
    }
}