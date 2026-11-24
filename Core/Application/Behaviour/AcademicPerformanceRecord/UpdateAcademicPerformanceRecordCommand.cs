using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.AcademicPerformanceRecords.Commands;

public sealed record UpdateAcademicPerformanceRecordCommand(
    int Id,
    string? AllDisciplineTeachers,
    int AvailableControlPoints,
    string? CurrentDisciplinePercent,
    int CurrentPointsFivePointScale,
    string? CurrentStudentAveragePercent,
    int FinalPoints,
    int FinalPointsFivePointScale,
    int ImportBatchId,
    int MaxAvailableControlPoints,
    int MaxDisciplinePoints,
    int MaxPointsByTopics,
    int? RetakePoints,
    int? RetakePointsNew,
    int SourceRowNumber,
    int TaskAndTestPoints,
    string? TeachersFromWorkload,
    string? TeachersWhoGraded,
    int DisciplineId,
    int OrganizationId,
    int SemesterId,
    int StudentId,
    int StudyGroupId,
    int SuborganizationId
) : IRequest<AcademicPerformanceRecordDto>;

public sealed class UpdateAcademicPerformanceRecordCommandHandler(IAcademicPerformanceRecordRepository repository)
    : IRequestHandler<UpdateAcademicPerformanceRecordCommand, AcademicPerformanceRecordDto>
{
    public async Task<AcademicPerformanceRecordDto> Handle(UpdateAcademicPerformanceRecordCommand request, CancellationToken cancellationToken)
    {
        AcademicPerformanceRecord? record = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (record is null)
        {
            throw new AcademicPerformanceRecordNotFoundException(request.Id);
        }

        bool recordExists = await repository.BatchRowExistsAsync(
            request.ImportBatchId,
            request.SourceRowNumber,
            request.Id,
            cancellationToken);

        if (recordExists)
        {
            throw new AcademicPerformanceRecordConflictException(request.ImportBatchId, request.SourceRowNumber);
        }

        record.AllDisciplineTeachers = request.AllDisciplineTeachers;
        record.AvailableControlPoints = request.AvailableControlPoints;
        record.CurrentDisciplinePercent = request.CurrentDisciplinePercent;
        record.CurrentPointsFivePointScale = request.CurrentPointsFivePointScale;
        record.CurrentStudentAveragePercent = request.CurrentStudentAveragePercent;
        record.FinalPoints = request.FinalPoints;
        record.FinalPointsFivePointScale = request.FinalPointsFivePointScale;
        record.ImportBatchId = request.ImportBatchId;
        record.MaxAvailableControlPoints = request.MaxAvailableControlPoints;
        record.MaxDisciplinePoints = request.MaxDisciplinePoints;
        record.MaxPointsByTopics = request.MaxPointsByTopics;
        record.RetakePoints = request.RetakePoints;
        record.RetakePointsNew = request.RetakePointsNew;
        record.SourceRowNumber = request.SourceRowNumber;
        record.TaskAndTestPoints = request.TaskAndTestPoints;
        record.TeachersFromWorkload = request.TeachersFromWorkload;
        record.TeachersWhoGraded = request.TeachersWhoGraded;
        record.DisciplineId = request.DisciplineId;
        record.OrganizationId = request.OrganizationId;
        record.SemesterId = request.SemesterId;
        record.StudentId = request.StudentId;
        record.StudyGroupId = request.StudyGroupId;
        record.SuborganizationId = request.SuborganizationId;

        await repository.UpdateAsync(record, cancellationToken);

        return AcademicPerformanceRecordDto.From(record);
    }
}