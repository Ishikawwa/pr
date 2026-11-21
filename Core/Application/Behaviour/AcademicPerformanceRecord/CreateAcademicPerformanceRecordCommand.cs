using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.AcademicPerformanceRecords.Commands;

public sealed record CreateAcademicPerformanceRecordCommand(
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

public sealed class CreateAcademicPerformanceRecordCommandHandler(IAcademicPerformanceRecordRepository repository)
    : IRequestHandler<CreateAcademicPerformanceRecordCommand, AcademicPerformanceRecordDto>
{
    public async Task<AcademicPerformanceRecordDto> Handle(CreateAcademicPerformanceRecordCommand request, CancellationToken cancellationToken)
    {
        bool recordExists = await repository.BatchRowExistsAsync(
            request.ImportBatchId,
            request.SourceRowNumber,
            null,
            cancellationToken);

        if (recordExists)
        {
            throw new AcademicPerformanceRecordConflictException(request.ImportBatchId, request.SourceRowNumber);
        }

        AcademicPerformanceRecord record = new AcademicPerformanceRecord
        {
            AllDisciplineTeachers = request.AllDisciplineTeachers,
            AvailableControlPoints = request.AvailableControlPoints,
            CurrentDisciplinePercent = request.CurrentDisciplinePercent,
            CurrentPointsFivePointScale = request.CurrentPointsFivePointScale,
            CurrentStudentAveragePercent = request.CurrentStudentAveragePercent,
            FinalPoints = request.FinalPoints,
            FinalPointsFivePointScale = request.FinalPointsFivePointScale,
            ImportBatchId = request.ImportBatchId,
            MaxAvailableControlPoints = request.MaxAvailableControlPoints,
            MaxDisciplinePoints = request.MaxDisciplinePoints,
            MaxPointsByTopics = request.MaxPointsByTopics,
            RetakePoints = request.RetakePoints,
            RetakePointsNew = request.RetakePointsNew,
            SourceRowNumber = request.SourceRowNumber,
            TaskAndTestPoints = request.TaskAndTestPoints,
            TeachersFromWorkload = request.TeachersFromWorkload,
            TeachersWhoGraded = request.TeachersWhoGraded,
            DisciplineId = request.DisciplineId,
            OrganizationId = request.OrganizationId,
            SemesterId = request.SemesterId,
            StudentId = request.StudentId,
            StudyGroupId = request.StudyGroupId,
            SuborganizationId = request.SuborganizationId
        };

        AcademicPerformanceRecord createdRecord = await repository.AddAsync(record, cancellationToken);

        return AcademicPerformanceRecordDto.From(createdRecord);
    }
}