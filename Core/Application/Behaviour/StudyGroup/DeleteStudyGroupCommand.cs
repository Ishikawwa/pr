using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.StudyGroups.Commands;

public sealed record DeleteStudyGroupCommand(int Id) : IRequest;

public sealed class DeleteStudyGroupCommandHandler(IStudyGroupRepository repository) : IRequestHandler<DeleteStudyGroupCommand>
{
    public async Task Handle(DeleteStudyGroupCommand request, CancellationToken cancellationToken)
    {
        StudyGroup? studyGroup = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (studyGroup is null)
        {
            throw new StudyGroupNotFoundException(request.Id);
        }

        await repository.DeleteAsync(studyGroup, cancellationToken);
    }
}