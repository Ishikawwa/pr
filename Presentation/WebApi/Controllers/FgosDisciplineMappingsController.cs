using Application.Behaviour.FgosDisciplineMappings;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.FgosDisciplineMappings;

namespace WebApi.Controllers;

[ApiController]
[Route("api/fgos-discipline-mappings")]
public sealed class FgosDisciplineMappingsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FgosDisciplineMappingDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] int? disciplineId = null, [FromQuery] int? fgosDisciplineId = null, [FromQuery] string? disciplineCode = null, [FromQuery] string? disciplineName = null, [FromQuery] string? fgosDisciplineCode = null, [FromQuery] string? fgosDisciplineName = null)
    {
        IReadOnlyList<FgosDisciplineMapping> mappings = await sender.Send(new GetFgosDisciplineMappingsQuery(disciplineId, fgosDisciplineId, disciplineCode, disciplineName, fgosDisciplineCode, fgosDisciplineName), cancellationToken);

        return Ok(mappings.Select(mapping => new FgosDisciplineMappingDto(
            mapping.Id,
            mapping.DisciplineId,
            mapping.Discipline.Code,
            mapping.Discipline.Name,
            mapping.FgosDisciplineId,
            mapping.FgosDiscipline.Code,
            mapping.FgosDiscipline.Name)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FgosDisciplineMappingDto>> GetById(int id, CancellationToken cancellationToken)
    {
        FgosDisciplineMapping? mapping = await sender.Send(new GetFgosDisciplineMappingByIdQuery(id), cancellationToken);

        if (mapping is null)
            return NotFound();

        return Ok(new FgosDisciplineMappingDto(
            mapping.Id,
            mapping.DisciplineId,
            mapping.Discipline.Code,
            mapping.Discipline.Name,
            mapping.FgosDisciplineId,
            mapping.FgosDiscipline.Code,
            mapping.FgosDiscipline.Name));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFgosDisciplineMappingDto dto, CancellationToken cancellationToken)
    {
        try
        {
            int id = await sender.Send(new CreateFgosDisciplineMappingCommand(dto.DisciplineId, dto.FgosDisciplineId), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось создать сопоставление. Возможно, такая связь уже существует или указаны несуществующие дисциплины.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateFgosDisciplineMappingDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new UpdateFgosDisciplineMappingCommand(id, dto.DisciplineId, dto.FgosDisciplineId), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить сопоставление. Возможно, такая связь уже существует или указаны несуществующие дисциплины.");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchFgosDisciplineMappingDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new PatchFgosDisciplineMappingCommand(id, dto.DisciplineId, dto.FgosDisciplineId), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить сопоставление. Возможно, такая связь уже существует или указаны несуществующие дисциплины.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        bool deleted = await sender.Send(new DeleteFgosDisciplineMappingCommand(id), cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}
