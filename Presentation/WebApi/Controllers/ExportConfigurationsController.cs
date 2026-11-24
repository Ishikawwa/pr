using Application.Behaviour.ExportConfigurations;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.ExportConfigurations;

namespace WebApi.Controllers;

[ApiController]
[Route("api/export-configurations")]
public sealed class ExportConfigurationsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ExportConfigurationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ExportConfigurationDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? templateKey,
        [FromQuery] int? studyGroupId,
        [FromQuery] bool? isActive,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<ExportConfiguration> configurations = await sender.Send(
            new GetExportConfigurationsQuery(search, templateKey, studyGroupId, isActive),
            cancellationToken);

        return Ok(configurations.Select(configuration => new ExportConfigurationDto(
            configuration.Id,
            configuration.ActiveVersionId,
            configuration.CreatedAt,
            configuration.IsActive,
            configuration.StudyGroupId,
            configuration.TemplateKey,
            configuration.UpdatedAt)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExportConfigurationDto>> GetById(int id, CancellationToken cancellationToken)
    {
        ExportConfiguration? configuration = await sender.Send(new GetExportConfigurationByIdQuery(id), cancellationToken);

        if (configuration is null)
            return NotFound();

        return Ok(new ExportConfigurationDto(
            configuration.Id,
            configuration.ActiveVersionId,
            configuration.CreatedAt,
            configuration.IsActive,
            configuration.StudyGroupId,
            configuration.TemplateKey,
            configuration.UpdatedAt));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExportConfigurationDto dto, CancellationToken cancellationToken)
    {
        int id = await sender.Send(
            new CreateExportConfigurationCommand(dto.TemplateKey, dto.StudyGroupId, dto.IsActive, dto.ActiveVersionId),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateExportConfigurationDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(
            new UpdateExportConfigurationCommand(id, dto.TemplateKey, dto.StudyGroupId, dto.IsActive, dto.ActiveVersionId),
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchExportConfigurationDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(
            new PatchExportConfigurationCommand(id, dto.TemplateKey, dto.StudyGroupId, dto.IsActive, dto.ActiveVersionId),
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteExportConfigurationCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить конфигурацию: существуют связанные версии или зависимости.");
        }
    }
}