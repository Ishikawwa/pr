using Application.Features.Students;
using Application.Features.Students.Commands;
using Application.Features.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/students")]
public sealed class StudentsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StudentDto>>> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<StudentDto> students = await sender.Send(new GetStudentsQuery(), cancellationToken);

        return Ok(students);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentDto>> GetById(int id, CancellationToken cancellationToken)
    {
        StudentDto student = await sender.Send(new GetStudentByIdQuery(id), cancellationToken);

        return Ok(student);
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create([FromBody] CreateStudentCommand command, CancellationToken cancellationToken)
    {
        StudentDto student = await sender.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<StudentDto>> Update(int id, [FromBody] UpdateStudentRequest request, CancellationToken cancellationToken)
    {
        UpdateStudentCommand command = new UpdateStudentCommand(
            id,
            request.Email,
            request.FirstName,
            request.LastName,
            request.Patronymic,
            request.AdmissionBase,
            request.AdmissionYear);

        StudentDto student = await sender.Send(command, cancellationToken);

        return Ok(student);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteStudentCommand(id), cancellationToken);

        return NoContent();
    }
}

public sealed record UpdateStudentRequest(
    string Email,
    string FirstName,
    string LastName,
    string? Patronymic,
    int? AdmissionBase,
    int? AdmissionYear);