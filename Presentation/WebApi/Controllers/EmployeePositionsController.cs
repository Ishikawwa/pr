using Application.Behaviour.EmployeePositions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos.EmployeePositions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/employee-positions")]
public sealed class EmployeePositionsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EmployeePositionDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] int? employeeId = null, [FromQuery] int? positionId = null, [FromQuery] string? employeeName = null, [FromQuery] string? positionTitle = null)
    {
        IReadOnlyList<EmployeePosition> employeePositions = await sender.Send(new GetEmployeePositionsQuery(employeeId, positionId, employeeName, positionTitle), cancellationToken);

        return Ok(employeePositions.Select(employeePosition => new EmployeePositionDto(
            employeePosition.Id,
            employeePosition.EmployeeId,
            employeePosition.Employee.FullName,
            employeePosition.PositionId,
            employeePosition.Position.Title)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeePositionDto>> GetById(int id, CancellationToken cancellationToken)
    {
        EmployeePosition? employeePosition = await sender.Send(new GetEmployeePositionByIdQuery(id), cancellationToken);

        if (employeePosition is null)
            return NotFound();

        return Ok(new EmployeePositionDto(
            employeePosition.Id,
            employeePosition.EmployeeId,
            employeePosition.Employee.FullName,
            employeePosition.PositionId,
            employeePosition.Position.Title));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeePositionDto dto, CancellationToken cancellationToken)
    {
        int id = await sender.Send(new CreateEmployeePositionCommand(dto.EmployeeId, dto.PositionId), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateEmployeePositionDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(new UpdateEmployeePositionCommand(id, dto.EmployeeId, dto.PositionId), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchEmployeePositionDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(new PatchEmployeePositionCommand(id, dto.EmployeeId, dto.PositionId), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        bool deleted = await sender.Send(new DeleteEmployeePositionCommand(id), cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}