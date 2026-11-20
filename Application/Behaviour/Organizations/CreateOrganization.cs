using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Behaviour.Organizations;

public sealed record CreateOrganizationCommand(string Name) : IRequest<int>;

public sealed class CreateOrganizationHandler(IOrganizationRepository repository, IValidator<CreateOrganizationCommand> validator) : IRequestHandler<CreateOrganizationCommand, int>
{
    public async Task<int> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        Organization organization = new() { Name = request.Name.Trim() };

        await repository.AddAsync(organization, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return organization.Id;
    }
}

public sealed class CreateOrganizationValidator : AbstractValidator<CreateOrganizationCommand>
{
    public CreateOrganizationValidator(IOrganizationRepository repository)
    {
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Название организации обязательно.")
            .MaximumLength(255).WithMessage("Название не должно превышать 255 символов.")
            .MustAsync(async (name, cancellationToken) =>
                !await repository.ExistsByNameAsync(name.Trim(), null, cancellationToken))
            .WithMessage("Организация с таким названием уже существует.");
    }
}