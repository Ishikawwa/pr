using Application.Behaviour.Disciplines;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.Disciplines;

namespace WebApi.Controllers;

[ApiController]
[Route("api/disciplines")]
public sealed class DisciplinesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DisciplineDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] string? code = null, [FromQuery] string? name = null, [FromQuery] int? suborganizationId = null)
    {
        IReadOnlyList<Discipline> disciplines = await sender.Send(new GetDisciplinesQuery(code, name, suborganizationId), cancellationToken);

        return Ok(disciplines.Select(discipline => new DisciplineDto(discipline.Id, discipline.Code, discipline.Name, discipline.SuborganizationId)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DisciplineDto>> GetById(int id, CancellationToken cancellationToken)
    {
        Discipline? discipline = await sender.Send(new GetDisciplineByIdQuery(id), cancellationToken);

        if (discipline is null)
            return NotFound();

        return Ok(new DisciplineDto(discipline.Id, discipline.Code, discipline.Name, discipline.SuborganizationId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateDisciplineDto dto, CancellationToken cancellationToken)
    {
        int id = await sender.Send(new CreateDisciplineCommand(dto.Code, dto.Name, dto.SuborganizationId), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateDisciplineDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(new UpdateDisciplineCommand(id, dto.Code, dto.Name, dto.SuborganizationId), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchDisciplineDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(new PatchDisciplineCommand(id, dto.Code, dto.Name, dto.SuborganizationId), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteDisciplineCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить дисциплину: существуют связанные записи.");
        }
    }
}
