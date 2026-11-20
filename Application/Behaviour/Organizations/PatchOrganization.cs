using Application.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Behaviour.Organizations;

public sealed record PatchOrganizationCommand(int Id, string? Name) : IRequest<bool>;

public sealed class PatchOrganizationHandler(IOrganizationRepository repository, IValidator<PatchOrganizationCommand> validator) : IRequestHandler<PatchOrganizationCommand, bool>
{
    public async Task<bool> Handle(PatchOrganizationCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        Domain.Entities.Organization? organization = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (organization is null)
            return false;

        if (request.Name is not null)
            organization.Name = request.Name.Trim();

        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public sealed class PatchOrganizationValidator : AbstractValidator<PatchOrganizationCommand>
{
    public PatchOrganizationValidator(IOrganizationRepository repository)
    {
        RuleFor(command => command.Id)
            .GreaterThan(0);

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Название организации не может быть пустым.")
            .MaximumLength(255).WithMessage("Название не должно превышать 255 символов.")
            .MustAsync(async (command, name, cancellationToken) =>
                name is null || !await repository.ExistsByNameAsync(name.Trim(), command.Id, cancellationToken))
            .WithMessage("Организация с таким названием уже существует.");
    }
}