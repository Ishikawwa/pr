
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.StudyGroups.Commands;

public sealed record PatchStudyGroupCommand(
    int Id,
    string? Name,
    int? SuborganizationId,
    int? StudyProgramId,
    int? StudyPlanId,
    bool HasName,
    bool HasSuborganizationId,
    bool HasStudyProgramId,
    bool HasStudyPlanId)
    : IRequest<StudyGroupDto>;

public sealed class PatchStudyGroupCommandValidator
    : AbstractValidator<PatchStudyGroupCommand>
{
    public PatchStudyGroupCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x)
            .Must(x =>
                x.HasName ||
                x.HasSuborganizationId ||
                x.HasStudyProgramId ||
                x.HasStudyPlanId)
            .WithMessage("At least one field must be provided.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .When(x => x.HasName);

        RuleFor(x => x.SuborganizationId)
            .Must(x => x.HasValue && x.Value > 0)
            .When(x => x.HasSuborganizationId);

        RuleFor(x => x.StudyProgramId)
            .Must(x => !x.HasValue || x.Value > 0)
            .When(x => x.HasStudyProgramId);

        RuleFor(x => x.StudyPlanId)
            .Must(x => !x.HasValue || x.Value > 0)
            .When(x => x.HasStudyPlanId);
    }
}

public sealed class PatchStudyGroupCommandHandler(
    IStudyGroupRepository repository,
    IValidator<PatchStudyGroupCommand> validator)
    : IRequestHandler<PatchStudyGroupCommand, StudyGroupDto>
{
    public async Task<StudyGroupDto> Handle(
        PatchStudyGroupCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        StudyGroup studyGroup = await repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (studyGroup is null)
        {
            throw new StudyGroupNotFoundException(request.Id);
        }

        if (request.HasName)
        {
            studyGroup.Name = request.Name!.Trim();
        }

        if (request.HasSuborganizationId)
        {
            studyGroup.SuborganizationId = request.SuborganizationId!.Value;
        }

        if (request.HasStudyProgramId)
        {
            studyGroup.StudyProgramId = request.StudyProgramId;
        }

        if (request.HasStudyPlanId)
        {
            studyGroup.StudyPlanId = request.StudyPlanId;
        }

        await repository.UpdateAsync(
            studyGroup,
            cancellationToken);

        return StudyGroupDto.From(studyGroup);
    }
}
