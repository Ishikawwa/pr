using Application.Behaviour.StudyPlanDisciplines;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.StudyPlanDisciplines;

namespace WebApi.Controllers;

[ApiController]
[Route("api/study-plan-disciplines")]
public sealed class StudyPlanDisciplinesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StudyPlanDisciplineDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] int? studyPlanId = null, [FromQuery] int? fgosDisciplineId = null, [FromQuery] int? semesterNumber = null, [FromQuery] string? controlForm = null, [FromQuery] string? fgosDisciplineCode = null, [FromQuery] string? fgosDisciplineName = null)
    {
        IReadOnlyList<StudyPlanDiscipline> studyPlanDisciplines = await sender.Send(new GetStudyPlanDisciplinesQuery(studyPlanId, fgosDisciplineId, semesterNumber, controlForm, fgosDisciplineCode, fgosDisciplineName), cancellationToken);

        return Ok(studyPlanDisciplines.Select(studyPlanDiscipline => new StudyPlanDisciplineDto(
            studyPlanDiscipline.Id,
            studyPlanDiscipline.StudyPlanId,
            studyPlanDiscipline.StudyPlan.Name,
            studyPlanDiscipline.FgosDisciplineId,
            studyPlanDiscipline.FgosDiscipline.Code,
            studyPlanDiscipline.FgosDiscipline.Name,
            studyPlanDiscipline.SemesterNumber,
            studyPlanDiscipline.PlannedHours,
            studyPlanDiscipline.ControlForm)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudyPlanDisciplineDto>> GetById(int id, CancellationToken cancellationToken)
    {
        StudyPlanDiscipline? studyPlanDiscipline = await sender.Send(new GetStudyPlanDisciplineByIdQuery(id), cancellationToken);

        if (studyPlanDiscipline is null)
            return NotFound();

        return Ok(new StudyPlanDisciplineDto(
            studyPlanDiscipline.Id,
            studyPlanDiscipline.StudyPlanId,
            studyPlanDiscipline.StudyPlan.Name,
            studyPlanDiscipline.FgosDisciplineId,
            studyPlanDiscipline.FgosDiscipline.Code,
            studyPlanDiscipline.FgosDiscipline.Name,
            studyPlanDiscipline.SemesterNumber,
            studyPlanDiscipline.PlannedHours,
            studyPlanDiscipline.ControlForm));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStudyPlanDisciplineDto dto, CancellationToken cancellationToken)
    {
        try
        {
            int id = await sender.Send(new CreateStudyPlanDisciplineCommand(dto.StudyPlanId, dto.FgosDisciplineId, dto.SemesterNumber, dto.PlannedHours, dto.ControlForm), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось добавить дисциплину в учебный план. Проверьте указанные ID учебного плана и дисциплины ФГОС.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateStudyPlanDisciplineDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new UpdateStudyPlanDisciplineCommand(id, dto.StudyPlanId, dto.FgosDisciplineId, dto.SemesterNumber, dto.PlannedHours, dto.ControlForm), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить дисциплину учебного плана. Проверьте указанные ID.");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchStudyPlanDisciplineDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new PatchStudyPlanDisciplineCommand(id, dto.StudyPlanId, dto.FgosDisciplineId, dto.SemesterNumber, dto.PlannedHours, dto.ControlForm), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить дисциплину учебного плана. Проверьте указанные ID.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        bool deleted = await sender.Send(new DeleteStudyPlanDisciplineCommand(id), cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}
