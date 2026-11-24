using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.Students.Commands;

public sealed record PatchStudentCommand(int Id, string? Email, string? FirstName, string? LastName, string? Patronymic, int? AdmissionBase, int? AdmissionYear) : IRequest<StudentDto>;

public sealed class PatchStudentCommandValidator : AbstractValidator<PatchStudentCommand>
{
    public PatchStudentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(320)
            .When(x => x.Email is not null);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(200)
            .When(x => x.FirstName is not null);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(200)
            .When(x => x.LastName is not null);

        RuleFor(x => x.Patronymic)
            .MaximumLength(200)
            .When(x => x.Patronymic is not null);

        RuleFor(x => x.AdmissionYear)
            .InclusiveBetween(1900, 2200)
            .When(x => x.AdmissionYear.HasValue);

        RuleFor(x => x)
            .Must(request =>
                request.Email is not null ||
                request.FirstName is not null ||
                request.LastName is not null ||
                request.Patronymic is not null ||
                request.AdmissionBase.HasValue ||
                request.AdmissionYear.HasValue)
            .WithMessage("At least one field must be provided.");
    }
}

public sealed class PatchStudentCommandHandler(
    IStudentRepository repository,
    IValidator<PatchStudentCommand> validator)
    : IRequestHandler<PatchStudentCommand, StudentDto>
{
    public async Task<StudentDto> Handle(PatchStudentCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        Student student = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (student is null)
            throw new StudentNotFoundException(request.Id);

        if (request.Email is not null)
        {
            string email = request.Email.Trim();

            bool emailExists = await repository.EmailExistsAsync(email, request.Id, cancellationToken);

            if (emailExists)
                throw new StudentEmailConflictException(email);

            student.Email = email;
        }

        if (request.FirstName is not null)
            student.FirstName = request.FirstName.Trim();

        if (request.LastName is not null)
            student.LastName = request.LastName.Trim();

        if (request.Patronymic is not null)
            student.Patronymic = request.Patronymic.Trim();

        if (request.AdmissionBase.HasValue)
            student.AdmissionBase = request.AdmissionBase.Value;

        if (request.AdmissionYear.HasValue)
            student.AdmissionYear = request.AdmissionYear.Value;

        await repository.UpdateAsync(student, cancellationToken);

        return StudentDto.From(student);
    }
}
