using Application.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Behaviour.Organizations;

public sealed record UpdateOrganizationCommand(int Id, string Name) : IRequest<bool>;

public sealed class UpdateOrganizationHandler(IOrganizationRepository repository, IValidator<UpdateOrganizationCommand> validator) : IRequestHandler<UpdateOrganizationCommand, bool>
{
    public async Task<bool> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        Domain.Entities.Organization? organization = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (organization is null)
            return false;

        organization.Name = request.Name.Trim();

        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public sealed class UpdateOrganizationValidator : AbstractValidator<UpdateOrganizationCommand>
{
    public UpdateOrganizationValidator(IOrganizationRepository repository)
    {
        RuleFor(command => command.Id)
            .GreaterThan(0);

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Название организации обязательно.")
            .MaximumLength(255).WithMessage("Название не должно превышать 255 символов.")
            .MustAsync(async (command, name, cancellationToken) =>
                !await repository.ExistsByNameAsync(name.Trim(), command.Id, cancellationToken))
            .WithMessage("Организация с таким названием уже существует.");
    }
}