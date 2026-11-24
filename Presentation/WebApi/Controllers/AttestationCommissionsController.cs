using Application.Behaviour.AttestationCommissions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.AttestationCommissions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/attestation-commissions")]
public sealed class AttestationCommissionsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AttestationCommissionDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] int? disciplineId = null, [FromQuery] int? studyGroupId = null, [FromQuery] int? studyProgramId = null, [FromQuery] string? academicYear = null, [FromQuery] string? semesterLabel = null, [FromQuery] int? semesterNumber = null, [FromQuery] string? disciplineName = null, [FromQuery] string? studyGroupName = null)
    {
        IReadOnlyList<AttestationCommission> commissions = await sender.Send(new GetAttestationCommissionsQuery(disciplineId, studyGroupId, studyProgramId, academicYear, semesterLabel, semesterNumber, disciplineName, studyGroupName), cancellationToken);

        return Ok(commissions.Select(commission => new AttestationCommissionDto(
            commission.Id,
            commission.DisciplineId,
            commission.Discipline?.Name,
            commission.StudyGroupId,
            commission.StudyGroup?.Name,
            commission.StudyProgramId,
            commission.StudyProgram.Name,
            commission.AcademicYear,
            commission.SemesterLabel,
            commission.SemesterNumber)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AttestationCommissionDto>> GetById(int id, CancellationToken cancellationToken)
    {
        AttestationCommission? commission = await sender.Send(new GetAttestationCommissionByIdQuery(id), cancellationToken);

        if (commission is null)
            return NotFound();

        return Ok(new AttestationCommissionDto(
            commission.Id,
            commission.DisciplineId,
            commission.Discipline?.Name,
            commission.StudyGroupId,
            commission.StudyGroup?.Name,
            commission.StudyProgramId,
            commission.StudyProgram.Name,
            commission.AcademicYear,
            commission.SemesterLabel,
            commission.SemesterNumber));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAttestationCommissionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            int id = await sender.Send(new CreateAttestationCommissionCommand(dto.DisciplineId, dto.StudyGroupId, dto.StudyProgramId, dto.AcademicYear, dto.SemesterLabel, dto.SemesterNumber), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось создать комиссию. Проверьте указанные ID программы, группы и дисциплины.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateAttestationCommissionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new UpdateAttestationCommissionCommand(id, dto.DisciplineId, dto.StudyGroupId, dto.StudyProgramId, dto.AcademicYear, dto.SemesterLabel, dto.SemesterNumber), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить комиссию. Проверьте указанные ID программы, группы и дисциплины.");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchAttestationCommissionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new PatchAttestationCommissionCommand(id, dto.DisciplineId, dto.StudyGroupId, dto.StudyProgramId, dto.AcademicYear, dto.SemesterLabel, dto.SemesterNumber), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить комиссию. Проверьте указанные ID программы, группы и дисциплины.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteAttestationCommissionCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить комиссию: существуют связанные записи.");
        }
    }
}