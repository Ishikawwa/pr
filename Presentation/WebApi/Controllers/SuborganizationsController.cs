
using Application.Behaviour.Suborganizations;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.Suborganizations;

namespace WebApi.Controllers;

[ApiController]
[Route("api/suborganizations")]
public sealed class SuborganizationsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SuborganizationDto>>> GetAll([FromQuery] string? name, [FromQuery] int? organizationId, CancellationToken cancellationToken)
    {
        IReadOnlyList<Suborganization> suborganizations = await sender.Send(new GetSuborganizationsQuery(name, organizationId), cancellationToken);

        return Ok(suborganizations.Select(suborganization => new SuborganizationDto(suborganization.Id, suborganization.Name, suborganization.OrganizationId)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SuborganizationDto>> GetById(int id, CancellationToken cancellationToken)
    {
        Suborganization? suborganization = await sender.Send(new GetSuborganizationByIdQuery(id), cancellationToken);

        if (suborganization is null)
            return NotFound();

        return Ok(new SuborganizationDto(suborganization.Id, suborganization.Name, suborganization.OrganizationId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSuborganizationDto dto, CancellationToken cancellationToken)
    {
        int id = await sender.Send(new CreateSuborganizationCommand(dto.Name, dto.OrganizationId), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateSuborganizationDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(new UpdateSuborganizationCommand(id, dto.Name, dto.OrganizationId), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchSuborganizationDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(new PatchSuborganizationCommand(id, dto.Name, dto.OrganizationId), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteSuborganizationCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить подразделение: существуют связанные записи.");
        }
    }
}
