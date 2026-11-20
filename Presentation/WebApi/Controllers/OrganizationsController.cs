using Application.Behaviour.Organizations;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.Organizations;

namespace WebApi.Controllers;

[ApiController]
[Route("api/organizations")]
public sealed class OrganizationsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<OrganizationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OrganizationDto>>> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<Organization> organizations = await sender.Send(new GetOrganizationsQuery(), cancellationToken);

        return Ok(organizations.Select(organization => new OrganizationDto(organization.Id, organization.Name)).ToList());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OrganizationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrganizationDto>> GetById(int id, CancellationToken cancellationToken)
    {
        Organization? organization = await sender.Send(new GetOrganizationByIdQuery(id), cancellationToken);

        if (organization is null)
            return NotFound();

        return Ok(new OrganizationDto(organization.Id, organization.Name));
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateOrganizationDto dto, CancellationToken cancellationToken)
    {
        try
        {
            int id = await sender.Send(new CreateOrganizationCommand(dto.Name), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Errors.Select(error => error.ErrorMessage));
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateOrganizationDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new UpdateOrganizationCommand(id, dto.Name), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Errors.Select(error => error.ErrorMessage));
        }
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(int id, PatchOrganizationDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new PatchOrganizationCommand(id, dto.Name), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Errors.Select(error => error.ErrorMessage));
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteOrganizationCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить организацию: существуют связанные записи.");
        }
    }
}