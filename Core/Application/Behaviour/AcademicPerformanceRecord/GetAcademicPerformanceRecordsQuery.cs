using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.AcademicPerformanceRecords.Queries;

public sealed record GetAcademicPerformanceRecordsQuery(
    int? ImportBatchId,
    int? SourceRowNumber,
    int? DisciplineId,
    int? OrganizationId,
    int? SemesterId,
    int? StudentId,
    int? StudyGroupId,
    int? SuborganizationId) : IRequest<IReadOnlyList<AcademicPerformanceRecordDto>>;

public sealed class GetAcademicPerformanceRecordsQueryHandler(IAcademicPerformanceRecordRepository repository)
    : IRequestHandler<GetAcademicPerformanceRecordsQuery, IReadOnlyList<AcademicPerformanceRecordDto>>
{
    public async Task<IReadOnlyList<AcademicPerformanceRecordDto>> Handle(GetAcademicPerformanceRecordsQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<AcademicPerformanceRecord> records = await repository.GetAllAsync(
            request.ImportBatchId,
            request.SourceRowNumber,
            request.DisciplineId,
            request.OrganizationId,
            request.SemesterId,
            request.StudentId,
            request.StudyGroupId,
            request.SuborganizationId,
            cancellationToken);

        return records.Select(AcademicPerformanceRecordDto.From).ToArray();
    }
}
