using Application.Behaviour.ImportBatches;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.ImportBatches;

namespace WebApi.Controllers;

[ApiController]
[Route("api/import-batches")]
public sealed class ImportBatchesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ImportBatchDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] string? semester = null, [FromQuery] string? sourceFileName = null, [FromQuery] DateTime? importedFrom = null, [FromQuery] DateTime? importedTo = null, [FromQuery] int? rowCount = null)
    {
        IReadOnlyList<ImportBatch> importBatches = await sender.Send(new GetImportBatchesQuery(semester, sourceFileName, importedFrom, importedTo, rowCount), cancellationToken);

        return Ok(importBatches.Select(importBatch => new ImportBatchDto(importBatch.Id, importBatch.ImportedAt, importBatch.RowCount, importBatch.Semester, importBatch.SourceFileName)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ImportBatchDto>> GetById(int id, CancellationToken cancellationToken)
    {
        ImportBatch? importBatch = await sender.Send(new GetImportBatchByIdQuery(id), cancellationToken);

        if (importBatch is null)
            return NotFound();

        return Ok(new ImportBatchDto(importBatch.Id, importBatch.ImportedAt, importBatch.RowCount, importBatch.Semester, importBatch.SourceFileName));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateImportBatchDto dto, CancellationToken cancellationToken)
    {
        try
        {
            int id = await sender.Send(new CreateImportBatchCommand(dto.Semester, dto.SourceFileName, dto.RowCount), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось создать пакет импорта из-за ограничений базы данных.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateImportBatchDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new UpdateImportBatchCommand(id, dto.ImportedAt, dto.RowCount, dto.Semester, dto.SourceFileName), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить пакет импорта из-за ограничений базы данных.");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchImportBatchDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new PatchImportBatchCommand(id, dto.ImportedAt, dto.RowCount, dto.Semester, dto.SourceFileName), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить пакет импорта из-за ограничений базы данных.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteImportBatchCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить пакет импорта из-за связанных записей или других ограничений базы данных.");
        }
    }
}