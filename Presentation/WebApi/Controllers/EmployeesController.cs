using Application.Behaviour.Employees;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.Employees;

namespace WebApi.Controllers;

[ApiController]
[Route("api/employees")]
public sealed class EmployeesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EmployeeDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] string? fullName = null)
    {
        IReadOnlyList<Employee> employees = await sender.Send(new GetEmployeesQuery(fullName), cancellationToken);

        return Ok(employees.Select(employee => new EmployeeDto(employee.Id, employee.FullName)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id, CancellationToken cancellationToken)
    {
        Employee? employee = await sender.Send(new GetEmployeeByIdQuery(id), cancellationToken);

        if (employee is null)
            return NotFound();

        return Ok(new EmployeeDto(employee.Id, employee.FullName));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeDto dto, CancellationToken cancellationToken)
    {
        int id = await sender.Send(new CreateEmployeeCommand(dto.FullName), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateEmployeeDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(new UpdateEmployeeCommand(id, dto.FullName), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchEmployeeDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(new PatchEmployeeCommand(id, dto.FullName), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteEmployeeCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить сотрудника: существуют связанные записи.");
        }
    }
}
