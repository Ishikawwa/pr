using Application.Behaviour.ExportTemplateVersions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.ExportTemplateVersions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/export-template-versions")]
public sealed class ExportTemplateVersionsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ExportTemplateVersionDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? templateKey,
        [FromQuery] string? format,
        [FromQuery] int? studyGroupId,
        [FromQuery] bool? isActive,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<ExportTemplateVersion> versions = await sender.Send(
            new GetExportTemplateVersionsQuery(search, templateKey, format, studyGroupId, isActive),
            cancellationToken);

        return Ok(versions.Select(version => new ExportTemplateVersionDto(
            version.Id,
            version.BasedOnVersionId,
            version.ContentBase64,
            version.CreatedAt,
            version.Format,
            version.IsActive,
            version.Name,
            version.OriginalFileName,
            version.StudyGroupId,
            version.TemplateKey,
            version.VersionNumber)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExportTemplateVersionDto>> GetById(int id, CancellationToken cancellationToken)
    {
        ExportTemplateVersion? version = await sender.Send(new GetExportTemplateVersionByIdQuery(id), cancellationToken);

        if (version is null)
            return NotFound();

        return Ok(new ExportTemplateVersionDto(
            version.Id,
            version.BasedOnVersionId,
            version.ContentBase64,
            version.CreatedAt,
            version.Format,
            version.IsActive,
            version.Name,
            version.OriginalFileName,
            version.StudyGroupId,
            version.TemplateKey,
            version.VersionNumber));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExportTemplateVersionDto dto, CancellationToken cancellationToken)
    {
        int id = await sender.Send(
            new CreateExportTemplateVersionCommand(
                dto.BasedOnVersionId,
                dto.ContentBase64,
                dto.Format,
                dto.IsActive,
                dto.Name,
                dto.OriginalFileName,
                dto.StudyGroupId,
                dto.TemplateKey,
                dto.VersionNumber),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateExportTemplateVersionDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(
            new UpdateExportTemplateVersionCommand(
                id,
                dto.BasedOnVersionId,
                dto.ContentBase64,
                dto.Format,
                dto.IsActive,
                dto.Name,
                dto.OriginalFileName,
                dto.StudyGroupId,
                dto.TemplateKey,
                dto.VersionNumber),
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchExportTemplateVersionDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(
            new PatchExportTemplateVersionCommand(
                id,
                dto.BasedOnVersionId,
                dto.ContentBase64,
                dto.Format,
                dto.IsActive,
                dto.Name,
                dto.OriginalFileName,
                dto.StudyGroupId,
                dto.TemplateKey,
                dto.VersionNumber),
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteExportTemplateVersionCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить версию шаблона: на неё ссылаются другие версии.");
        }
    }
}