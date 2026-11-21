using Application.Behaviour.StudyPlans;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.StudyPlans;

namespace WebApi.Controllers;

[ApiController]
[Route("api/study-plans")]
public sealed class StudyPlansController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StudyPlanDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] string? name = null, [FromQuery] int? admissionBase = null, [FromQuery] int? admissionYear = null, [FromQuery] int? studyProgramId = null)
    {
        IReadOnlyList<StudyPlan> studyPlans = await sender.Send(new GetStudyPlansQuery(name, admissionBase, admissionYear, studyProgramId), cancellationToken);

        return Ok(studyPlans.Select(studyPlan => new StudyPlanDto(studyPlan.Id, studyPlan.AdmissionBase, studyPlan.AdmissionYear, studyPlan.Name, studyPlan.SourceFileName, studyPlan.StudyProgramId)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudyPlanDto>> GetById(int id, CancellationToken cancellationToken)
    {
        StudyPlan? studyPlan = await sender.Send(new GetStudyPlanByIdQuery(id), cancellationToken);

        if (studyPlan is null)
            return NotFound();

        return Ok(new StudyPlanDto(studyPlan.Id, studyPlan.AdmissionBase, studyPlan.AdmissionYear, studyPlan.Name, studyPlan.SourceFileName, studyPlan.StudyProgramId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStudyPlanDto dto, CancellationToken cancellationToken)
    {
        try
        {
            int id = await sender.Send(new CreateStudyPlanCommand(dto.AdmissionBase, dto.AdmissionYear, dto.Name, dto.SourceFileName, dto.StudyProgramId), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось создать учебный план. Проверьте образовательную программу и уникальность набора.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateStudyPlanDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new UpdateStudyPlanCommand(id, dto.AdmissionBase, dto.AdmissionYear, dto.Name, dto.SourceFileName, dto.StudyProgramId), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить учебный план. Проверьте образовательную программу и уникальность набора.");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchStudyPlanDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new PatchStudyPlanCommand(id, dto.AdmissionBase, dto.AdmissionYear, dto.Name, dto.SourceFileName, dto.StudyProgramId), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить учебный план. Проверьте образовательную программу и уникальность набора.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteStudyPlanCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить учебный план: существуют связанные группы или дисциплины.");
        }
    }
}
