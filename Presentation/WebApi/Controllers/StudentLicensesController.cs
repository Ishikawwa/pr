using Application.Behaviour.StudentLicenses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.StudentLicenses;

namespace WebApi.Controllers;

[ApiController]
[Route("api/student-licenses")]
public sealed class StudentLicensesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StudentLicenseDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] int? studentId = null, [FromQuery] int? licenseId = null, [FromQuery] string? studentName = null, [FromQuery] string? licenseName = null)
    {
        IReadOnlyList<StudentLicense> studentLicenses = await sender.Send(new GetStudentLicensesQuery(studentId, licenseId, studentName, licenseName), cancellationToken);

        return Ok(studentLicenses.Select(studentLicense => new StudentLicenseDto(
            studentLicense.Id,
            studentLicense.StudentId,
            $"{studentLicense.Student.LastName} {studentLicense.Student.FirstName} {studentLicense.Student.Patronymic}".Trim(),
            studentLicense.LicenseId,
            studentLicense.License.Name)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentLicenseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        StudentLicense? studentLicense = await sender.Send(new GetStudentLicenseByIdQuery(id), cancellationToken);

        if (studentLicense is null)
            return NotFound();

        return Ok(new StudentLicenseDto(
            studentLicense.Id,
            studentLicense.StudentId,
            $"{studentLicense.Student.LastName} {studentLicense.Student.FirstName} {studentLicense.Student.Patronymic}".Trim(),
            studentLicense.LicenseId,
            studentLicense.License.Name));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStudentLicenseDto dto, CancellationToken cancellationToken)
    {
        try
        {
            int id = await sender.Send(new CreateStudentLicenseCommand(dto.StudentId, dto.LicenseId), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось создать связь. Проверьте указанные ID студента и лицензии.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateStudentLicenseDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new UpdateStudentLicenseCommand(id, dto.StudentId, dto.LicenseId), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить связь. Проверьте указанные ID студента и лицензии.");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchStudentLicenseDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new PatchStudentLicenseCommand(id, dto.StudentId, dto.LicenseId), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить связь. Проверьте указанные ID студента и лицензии.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteStudentLicenseCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить связь: существуют связанные записи истории лицензии.");
        }
    }
}
