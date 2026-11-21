using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.AcademicPerformanceRecords.Queries;

public sealed record GetAcademicPerformanceRecordsQuery : IRequest<IReadOnlyList<AcademicPerformanceRecordDto>>;

public sealed class GetAcademicPerformanceRecordsQueryHandler(IAcademicPerformanceRecordRepository repository)
    : IRequestHandler<GetAcademicPerformanceRecordsQuery, IReadOnlyList<AcademicPerformanceRecordDto>>
{
    public async Task<IReadOnlyList<AcademicPerformanceRecordDto>> Handle(GetAcademicPerformanceRecordsQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<AcademicPerformanceRecord> records = await repository.GetAllAsync(cancellationToken);

        return records.Select(AcademicPerformanceRecordDto.From).ToArray();
    }
}