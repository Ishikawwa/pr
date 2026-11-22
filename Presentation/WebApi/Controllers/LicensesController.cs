using Application.Behaviour.Licenses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.Licenses;

namespace WebApi.Controllers;

[ApiController]
[Route("api/licenses")]
public sealed class LicensesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LicenseDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] string? name = null)
    {
        IReadOnlyList<License> licenses = await sender.Send(new GetLicensesQuery(name), cancellationToken);

        return Ok(licenses.Select(license => new LicenseDto(license.Id, license.Name)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LicenseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        License? license = await sender.Send(new GetLicenseByIdQuery(id), cancellationToken);

        if (license is null)
            return NotFound();

        return Ok(new LicenseDto(license.Id, license.Name));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLicenseDto dto, CancellationToken cancellationToken)
    {
        try
        {
            int id = await sender.Send(new CreateLicenseCommand(dto.Name), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось создать лицензию. Возможно, лицензия с таким названием уже существует.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateLicenseDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new UpdateLicenseCommand(id, dto.Name), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить лицензию. Возможно, лицензия с таким названием уже существует.");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchLicenseDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new PatchLicenseCommand(id, dto.Name), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить лицензию. Возможно, лицензия с таким названием уже существует.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteLicenseCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить лицензию: она используется в записях студентов.");
        }
    }
}
