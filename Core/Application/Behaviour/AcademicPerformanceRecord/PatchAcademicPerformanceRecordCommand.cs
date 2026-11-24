using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.AcademicPerformanceRecords.Commands;

public sealed record PatchAcademicPerformanceRecordCommand(
    int Id,
    string? AllDisciplineTeachers,
    int? AvailableControlPoints,
    string? CurrentDisciplinePercent,
    int? CurrentPointsFivePointScale,
    string? CurrentStudentAveragePercent,
    int? FinalPoints,
    int? FinalPointsFivePointScale,
    int? ImportBatchId,
    int? MaxAvailableControlPoints,
    int? MaxDisciplinePoints,
    int? MaxPointsByTopics,
    int? RetakePoints,
    int? RetakePointsNew,
    int? SourceRowNumber,
    int? TaskAndTestPoints,
    string? TeachersFromWorkload,
    string? TeachersWhoGraded,
    int? DisciplineId,
    int? OrganizationId,
    int? SemesterId,
    int? StudentId,
    int? StudyGroupId,
    int? SuborganizationId) : IRequest<AcademicPerformanceRecordDto>;

public sealed class PatchAcademicPerformanceRecordCommandHandler(IAcademicPerformanceRecordRepository repository)
    : IRequestHandler<PatchAcademicPerformanceRecordCommand, AcademicPerformanceRecordDto>
{
    public async Task<AcademicPerformanceRecordDto> Handle(PatchAcademicPerformanceRecordCommand request, CancellationToken cancellationToken)
    {
        AcademicPerformanceRecord record = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (record is null)
            throw new AcademicPerformanceRecordNotFoundException(request.Id);

        int importBatchId = request.ImportBatchId ?? record.ImportBatchId;
        int sourceRowNumber = request.SourceRowNumber ?? record.SourceRowNumber;

        if ((request.ImportBatchId.HasValue || request.SourceRowNumber.HasValue) &&
            await repository.BatchRowExistsAsync(importBatchId, sourceRowNumber, request.Id, cancellationToken))
            throw new AcademicPerformanceRecordConflictException(importBatchId, sourceRowNumber);

        if (request.AllDisciplineTeachers is not null)
            record.AllDisciplineTeachers = request.AllDisciplineTeachers;

        if (request.AvailableControlPoints.HasValue)
            record.AvailableControlPoints = request.AvailableControlPoints.Value;

        if (request.CurrentDisciplinePercent is not null)
            record.CurrentDisciplinePercent = request.CurrentDisciplinePercent;

        if (request.CurrentPointsFivePointScale.HasValue)
            record.CurrentPointsFivePointScale = request.CurrentPointsFivePointScale.Value;

        if (request.CurrentStudentAveragePercent is not null)
            record.CurrentStudentAveragePercent = request.CurrentStudentAveragePercent;

        if (request.FinalPoints.HasValue)
            record.FinalPoints = request.FinalPoints.Value;

        if (request.FinalPointsFivePointScale.HasValue)
            record.FinalPointsFivePointScale = request.FinalPointsFivePointScale.Value;

        if (request.ImportBatchId.HasValue)
            record.ImportBatchId = request.ImportBatchId.Value;

        if (request.MaxAvailableControlPoints.HasValue)
            record.MaxAvailableControlPoints = request.MaxAvailableControlPoints.Value;

        if (request.MaxDisciplinePoints.HasValue)
            record.MaxDisciplinePoints = request.MaxDisciplinePoints.Value;

        if (request.MaxPointsByTopics.HasValue)
            record.MaxPointsByTopics = request.MaxPointsByTopics.Value;

        if (request.RetakePoints.HasValue)
            record.RetakePoints = request.RetakePoints.Value;

        if (request.RetakePointsNew.HasValue)
            record.RetakePointsNew = request.RetakePointsNew.Value;

        if (request.SourceRowNumber.HasValue)
            record.SourceRowNumber = request.SourceRowNumber.Value;

        if (request.TaskAndTestPoints.HasValue)
            record.TaskAndTestPoints = request.TaskAndTestPoints.Value;

        if (request.TeachersFromWorkload is not null)
            record.TeachersFromWorkload = request.TeachersFromWorkload;

        if (request.TeachersWhoGraded is not null)
            record.TeachersWhoGraded = request.TeachersWhoGraded;

        if (request.DisciplineId.HasValue)
            record.DisciplineId = request.DisciplineId.Value;

        if (request.OrganizationId.HasValue)
            record.OrganizationId = request.OrganizationId.Value;

        if (request.SemesterId.HasValue)
            record.SemesterId = request.SemesterId.Value;

        if (request.StudentId.HasValue)
            record.StudentId = request.StudentId.Value;

        if (request.StudyGroupId.HasValue)
            record.StudyGroupId = request.StudyGroupId.Value;

        if (request.SuborganizationId.HasValue)
            record.SuborganizationId = request.SuborganizationId.Value;

        await repository.UpdateAsync(record, cancellationToken);

        return AcademicPerformanceRecordDto.From(record);
    }
}
