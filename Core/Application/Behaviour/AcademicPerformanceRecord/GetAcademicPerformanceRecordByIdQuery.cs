using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.AcademicPerformanceRecords.Queries;

public sealed record GetAcademicPerformanceRecordByIdQuery(int Id) : IRequest<AcademicPerformanceRecordDto>;

public sealed class GetAcademicPerformanceRecordByIdQueryHandler(IAcademicPerformanceRecordRepository repository)
    : IRequestHandler<GetAcademicPerformanceRecordByIdQuery, AcademicPerformanceRecordDto>
{
    public async Task<AcademicPerformanceRecordDto> Handle(GetAcademicPerformanceRecordByIdQuery request, CancellationToken cancellationToken)
    {
        AcademicPerformanceRecord? record = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (record is null)
        {
            throw new AcademicPerformanceRecordNotFoundException(request.Id);
        }

        return AcademicPerformanceRecordDto.From(record);
    }
}