using Application.Behaviour.AttestationCommissionMembers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.AttestationCommissionMembers;

namespace WebApi.Controllers;

[ApiController]
[Route("api/attestation-commission-members")]
public sealed class AttestationCommissionMembersController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AttestationCommissionMemberDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] int? commissionId = null, [FromQuery] string? fullName = null, [FromQuery] string? position = null, [FromQuery] string? role = null, [FromQuery] int? sortOrder = null)
    {
        IReadOnlyList<AttestationCommissionMember> members = await sender.Send(new GetAttestationCommissionMembersQuery(commissionId, fullName, position, role, sortOrder), cancellationToken);

        return Ok(members.Select(member => new AttestationCommissionMemberDto(member.Id, member.CommissionId, member.FullName, member.Position, member.Role, member.SortOrder)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AttestationCommissionMemberDto>> GetById(int id, CancellationToken cancellationToken)
    {
        AttestationCommissionMember? member = await sender.Send(new GetAttestationCommissionMemberByIdQuery(id), cancellationToken);

        if (member is null)
            return NotFound();

        return Ok(new AttestationCommissionMemberDto(member.Id, member.CommissionId, member.FullName, member.Position, member.Role, member.SortOrder));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAttestationCommissionMemberDto dto, CancellationToken cancellationToken)
    {
        try
        {
            int id = await sender.Send(new CreateAttestationCommissionMemberCommand(dto.CommissionId, dto.FullName, dto.Position, dto.Role, dto.SortOrder), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось добавить участника комиссии. Возможно, указан несуществующий ID комиссии или нарушено ограничение базы данных.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateAttestationCommissionMemberDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new UpdateAttestationCommissionMemberCommand(id, dto.CommissionId, dto.FullName, dto.Position, dto.Role, dto.SortOrder), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить участника комиссии. Проверьте ID комиссии и ограничения базы данных.");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchAttestationCommissionMemberDto dto, CancellationToken cancellationToken)
    {
        try
        {
            bool updated = await sender.Send(new PatchAttestationCommissionMemberCommand(id, dto.CommissionId, dto.FullName, dto.Position, dto.Role, dto.SortOrder), cancellationToken);

            return updated ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Не удалось обновить участника комиссии. Проверьте ID комиссии и ограничения базы данных.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteAttestationCommissionMemberCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить участника комиссии из-за ограничений базы данных.");
        }
    }
}