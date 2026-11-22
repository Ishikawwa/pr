using Application.Behaviour.StudentLicenseHistories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.StudentLicenseHistories;

namespace WebApi.Controllers;

[ApiController]
[Route("api/student-license-histories")]
public sealed class StudentLicenseHistoriesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StudentLicenseHistoryDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] int? studentLicenseId = null, [FromQuery] int? studentId = null, [FromQuery] int? licenseId = null, [FromQuery] string? academicYear = null, [FromQuery] int? semesterNumber = null, [FromQuery] string? semesterLabel = null, [FromQuery] string? studentName = null, [FromQuery] string? licenseName = null)
    {
        IReadOnlyList<StudentLicenseHistory> histories = await sender.Send(new GetStudentLicenseHistoriesQuery(studentLicenseId, studentId, licenseId, academicYear, semesterNumber, semesterLabel, studentName, licenseName), cancellationToken);

        return Ok(histories.Select(history => new StudentLicenseHistoryDto(
            history.Id,
            history.StudentLicenseId,
            history.StudentLicense.StudentId,
            $"{history.StudentLicense.Student.LastName} {history.StudentLicense.Student.FirstName} {history.StudentLicense.Student.Patronymic}".Trim(),
            history.StudentLicense.LicenseId,
            history.StudentLicense.License.Name,
            history.AcademicYear,
            history.SemesterNumber,
            history.SemesterLabel)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentLicenseHistoryDto>> GetById(int id, CancellationToken cancellationToken)
    {
        StudentLicenseHistory? history = await sender.Send(new GetStudentLicenseHistoryByIdQuery(id), cancellationToken);

        if (history is null)
            return NotFound();

        return Ok(new StudentLicenseHistoryDto(
            history.Id,
            history.StudentLicenseId,
            history.StudentLicense.StudentId,
            $"{history.StudentLicense.Student.LastName} {history.StudentLicense.Student.FirstName} {history.StudentLicense.Student.Patronymic}".Trim(),
            history.StudentLicense.LicenseId,
            history.StudentLicense.License.Name,
            history.AcademicYear,
            history.SemesterNumber,
            history.SemesterLabel));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStudentLicenseHistoryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            int id = await sender.Send(new CreateStudentLicenseHistoryCommand(dto.StudentLicenseId, dto.AcademicYear, dto.SemesterNumber, dto.SemesterLabel), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось создать запись истории. Проверьте указанный ID связи студента с лицензией.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateStudentLicenseHistoryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new UpdateStudentLicenseHistoryCommand(id, dto.StudentLicenseId, dto.AcademicYear, dto.SemesterNumber, dto.SemesterLabel), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить запись истории. Проверьте указанный ID связи студента с лицензией.");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchStudentLicenseHistoryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new PatchStudentLicenseHistoryCommand(id, dto.StudentLicenseId, dto.AcademicYear, dto.SemesterNumber, dto.SemesterLabel), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить запись истории. Проверьте указанный ID связи студента с лицензией.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        bool deleted = await sender.Send(new DeleteStudentLicenseHistoryCommand(id), cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}
