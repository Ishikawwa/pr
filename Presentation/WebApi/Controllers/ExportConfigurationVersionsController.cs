using Application.Behaviour.ExportConfigurationVersions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.ExportConfigurationVersions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/export-configuration-versions")]
public sealed class ExportConfigurationVersionsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ExportConfigurationVersionDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] int? configurationId,
        [FromQuery] int? versionNumber,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<ExportConfigurationVersion> versions = await sender.Send(
            new GetExportConfigurationVersionsQuery(search, configurationId, versionNumber),
            cancellationToken);

        return Ok(versions.Select(version => new ExportConfigurationVersionDto(
            version.Id,
            version.ConfigurationId,
            version.CreatedAt,
            version.Name,
            version.Settings,
            version.VersionNumber,
            version.ChangeNote)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExportConfigurationVersionDto>> GetById(int id, CancellationToken cancellationToken)
    {
        ExportConfigurationVersion? version = await sender.Send(new GetExportConfigurationVersionByIdQuery(id), cancellationToken);

        if (version is null)
            return NotFound();

        return Ok(new ExportConfigurationVersionDto(
            version.Id,
            version.ConfigurationId,
            version.CreatedAt,
            version.Name,
            version.Settings,
            version.VersionNumber,
            version.ChangeNote));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExportConfigurationVersionDto dto, CancellationToken cancellationToken)
    {
        int id = await sender.Send(
            new CreateExportConfigurationVersionCommand(dto.ConfigurationId, dto.Name, dto.Settings, dto.VersionNumber, dto.ChangeNote),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateExportConfigurationVersionDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(
            new UpdateExportConfigurationVersionCommand(id, dto.ConfigurationId, dto.Name, dto.Settings, dto.VersionNumber, dto.ChangeNote),
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchExportConfigurationVersionDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(
            new PatchExportConfigurationVersionCommand(id, dto.ConfigurationId, dto.Name, dto.Settings, dto.VersionNumber, dto.ChangeNote),
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteExportConfigurationVersionCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить версию: она используется конфигурацией экспорта.");
        }
    }
}