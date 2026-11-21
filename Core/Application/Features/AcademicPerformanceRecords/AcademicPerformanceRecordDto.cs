using Domain.Entities;

namespace Application.Features.AcademicPerformanceRecords;

public sealed record AcademicPerformanceRecordDto(
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
    int SuborganizationId)
{
    public static AcademicPerformanceRecordDto From(AcademicPerformanceRecord record)
    {
        return new AcademicPerformanceRecordDto(
            record.Id,
            record.AllDisciplineTeachers,
            record.AvailableControlPoints,
            record.CurrentDisciplinePercent,
            record.CurrentPointsFivePointScale,
            record.CurrentStudentAveragePercent,
            record.FinalPoints,
            record.FinalPointsFivePointScale,
            record.ImportBatchId,
            record.MaxAvailableControlPoints,
            record.MaxDisciplinePoints,
            record.MaxPointsByTopics,
            record.RetakePoints,
            record.RetakePointsNew,
            record.SourceRowNumber,
            record.TaskAndTestPoints,
            record.TeachersFromWorkload,
            record.TeachersWhoGraded,
            record.DisciplineId,
            record.OrganizationId,
            record.SemesterId,
            record.StudentId,
            record.StudyGroupId,
            record.SuborganizationId
        );
    }
}