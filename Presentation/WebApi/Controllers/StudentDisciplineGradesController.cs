using Application.Features.StudentDisciplineGrades;
using Application.Features.StudentDisciplineGrades.Commands;
using Application.Features.StudentDisciplineGrades.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/student-discipline-grades")]
public sealed class StudentDisciplineGradesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StudentDisciplineGradeDto>>> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<StudentDisciplineGradeDto> studentDisciplineGrades = await sender.Send(
            new GetStudentDisciplineGradesQuery(),
            cancellationToken);

        return Ok(studentDisciplineGrades);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentDisciplineGradeDto>> GetById(int id, CancellationToken cancellationToken)
    {
        StudentDisciplineGradeDto studentDisciplineGrade = await sender.Send(
            new GetStudentDisciplineGradeByIdQuery(id),
            cancellationToken);

        return Ok(studentDisciplineGrade);
    }

    [HttpPost]
    public async Task<ActionResult<StudentDisciplineGradeDto>> Create(
        [FromBody] CreateStudentDisciplineGradeCommand command,
        CancellationToken cancellationToken)
    {
        StudentDisciplineGradeDto studentDisciplineGrade = await sender.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = studentDisciplineGrade.Id },
            studentDisciplineGrade);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<StudentDisciplineGradeDto>> Update(
        int id,
        [FromBody] UpdateStudentDisciplineGradeRequest request,
        CancellationToken cancellationToken)
    {
        UpdateStudentDisciplineGradeCommand command = new UpdateStudentDisciplineGradeCommand(
            id,
            request.StudentId,
            request.DisciplineId,
            request.EmployeeId,
            request.AcademicPerformanceId,
            request.AdmissionYear,
            request.SemesterNumber,
            request.Grade,
            request.Date);

        StudentDisciplineGradeDto studentDisciplineGrade = await sender.Send(command, cancellationToken);

        return Ok(studentDisciplineGrade);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await sender.Send(
            new DeleteStudentDisciplineGradeCommand(id),
            cancellationToken);

        return NoContent();
    }
}

public sealed record UpdateStudentDisciplineGradeRequest(
    int StudentId,
    int DisciplineId,
    int EmployeeId,
    int? AcademicPerformanceId,
    int AdmissionYear,
    int SemesterNumber,
    string Grade,
    DateTime Date);