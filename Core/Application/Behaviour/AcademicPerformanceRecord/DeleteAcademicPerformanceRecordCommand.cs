using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.AcademicPerformanceRecords.Commands;

public sealed record DeleteAcademicPerformanceRecordCommand(int Id) : IRequest;

public sealed class DeleteAcademicPerformanceRecordCommandHandler(IAcademicPerformanceRecordRepository repository)
    : IRequestHandler<DeleteAcademicPerformanceRecordCommand>
{
    public async Task Handle(DeleteAcademicPerformanceRecordCommand request, CancellationToken cancellationToken)
    {
        AcademicPerformanceRecord? record = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (record is null)
        {
            throw new AcademicPerformanceRecordNotFoundException(request.Id);
        }

        await repository.DeleteAsync(record, cancellationToken);
    }
}