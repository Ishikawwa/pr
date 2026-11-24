using Application.Behaviour.Positions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.Positions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/positions")]
public sealed class PositionsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PositionDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] string? title = null)
    {
        IReadOnlyList<Position> positions = await sender.Send(new GetPositionsQuery(title), cancellationToken);

        return Ok(positions.Select(position => new PositionDto(position.Id, position.Title)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PositionDto>> GetById(int id, CancellationToken cancellationToken)
    {
        Position? position = await sender.Send(new GetPositionByIdQuery(id), cancellationToken);

        if (position is null)
            return NotFound();

        return Ok(new PositionDto(position.Id, position.Title));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePositionDto dto, CancellationToken cancellationToken)
    {
        int id = await sender.Send(new CreatePositionCommand(dto.Title), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdatePositionDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(new UpdatePositionCommand(id, dto.Title), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchPositionDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(new PatchPositionCommand(id, dto.Title), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeletePositionCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить должность: существуют связанные записи о назначениях сотрудников.");
        }
    }
}
