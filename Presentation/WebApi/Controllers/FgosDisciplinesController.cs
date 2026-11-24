
using Application.Behaviour.FgosDisciplines;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.FgosDisciplines;

namespace WebApi.Controllers;

[ApiController]
[Route("api/fgos-disciplines")]
public sealed class FgosDisciplinesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FgosDisciplineDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] string? name = null, [FromQuery] string? code = null, [FromQuery] int? studyProgramId = null)
    {
        IReadOnlyList<FgosDiscipline> fgosDisciplines = await sender.Send(new GetFgosDisciplinesQuery(name, code, studyProgramId), cancellationToken);

        return Ok(fgosDisciplines.Select(fgosDiscipline => new FgosDisciplineDto(fgosDiscipline.Id, fgosDiscipline.Code, fgosDiscipline.Name, fgosDiscipline.StudyProgramId)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FgosDisciplineDto>> GetById(int id, CancellationToken cancellationToken)
    {
        FgosDiscipline? fgosDiscipline = await sender.Send(new GetFgosDisciplineByIdQuery(id), cancellationToken);

        if (fgosDiscipline is null)
            return NotFound();

        return Ok(new FgosDisciplineDto(fgosDiscipline.Id, fgosDiscipline.Code, fgosDiscipline.Name, fgosDiscipline.StudyProgramId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFgosDisciplineDto dto, CancellationToken cancellationToken)
    {
        try
        {
            int id = await sender.Send(new CreateFgosDisciplineCommand(dto.Code, dto.Name, dto.StudyProgramId), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось создать дисциплину ФГОС. Проверьте образовательную программу и ограничения базы данных.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateFgosDisciplineDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new UpdateFgosDisciplineCommand(id, dto.Code, dto.Name, dto.StudyProgramId), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить дисциплину ФГОС. Проверьте образовательную программу и ограничения базы данных.");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchFgosDisciplineDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new PatchFgosDisciplineCommand(id, dto.Code, dto.Name, dto.StudyProgramId), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить дисциплину ФГОС. Проверьте образовательную программу и ограничения базы данных.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteFgosDisciplineCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить дисциплину ФГОС: существуют связанные записи.");
        }
    }
}
