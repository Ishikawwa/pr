using Application.Features.AcademicPerformanceRecords;
using Application.Features.AcademicPerformanceRecords.Commands;
using Application.Features.AcademicPerformanceRecords.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/academic-performance-records")]
public sealed class AcademicPerformanceRecordsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AcademicPerformanceRecordDto>>> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<AcademicPerformanceRecordDto> records = await sender.Send(
            new GetAcademicPerformanceRecordsQuery(),
            cancellationToken);

        return Ok(records);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AcademicPerformanceRecordDto>> GetById(int id, CancellationToken cancellationToken)
    {
        AcademicPerformanceRecordDto record = await sender.Send(
            new GetAcademicPerformanceRecordByIdQuery(id),
            cancellationToken);

        return Ok(record);
    }

    [HttpPost]
    public async Task<ActionResult<AcademicPerformanceRecordDto>> Create(
        [FromBody] CreateAcademicPerformanceRecordCommand command,
        CancellationToken cancellationToken)
    {
        AcademicPerformanceRecordDto record = await sender.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AcademicPerformanceRecordDto>> Update(
        int id,
        [FromBody] UpdateAcademicPerformanceRecordRequest request,
        CancellationToken cancellationToken)
    {
        UpdateAcademicPerformanceRecordCommand command = new UpdateAcademicPerformanceRecordCommand(
            id,
            request.AllDisciplineTeachers,
            request.AvailableControlPoints,
            request.CurrentDisciplinePercent,
            request.CurrentPointsFivePointScale,
            request.CurrentStudentAveragePercent,
            request.FinalPoints,
            request.FinalPointsFivePointScale,
            request.ImportBatchId,
            request.MaxAvailableControlPoints,
            request.MaxDisciplinePoints,
            request.MaxPointsByTopics,
            request.RetakePoints,
            request.RetakePointsNew,
            request.SourceRowNumber,
            request.TaskAndTestPoints,
            request.TeachersFromWorkload,
            request.TeachersWhoGraded,
            request.DisciplineId,
            request.OrganizationId,
            request.SemesterId,
            request.StudentId,
            request.StudyGroupId,
            request.SuborganizationId);

        AcademicPerformanceRecordDto record = await sender.Send(command, cancellationToken);

        return Ok(record);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteAcademicPerformanceRecordCommand(id), cancellationToken);

        return NoContent();
    }
}

public sealed record UpdateAcademicPerformanceRecordRequest(
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
    int SuborganizationId);