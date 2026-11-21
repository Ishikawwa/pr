using Application.Features.StudyGroups;
using Application.Features.StudyGroups.Commands;
using Application.Features.StudyGroups.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/study-groups")]
public sealed class StudyGroupsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StudyGroupDto>>> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<StudyGroupDto> studyGroups = await sender.Send(new GetStudyGroupsQuery(), cancellationToken);

        return Ok(studyGroups);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudyGroupDto>> GetById(int id, CancellationToken cancellationToken)
    {
        StudyGroupDto studyGroup = await sender.Send(new GetStudyGroupByIdQuery(id), cancellationToken);

        return Ok(studyGroup);
    }

    [HttpPost]
    public async Task<ActionResult<StudyGroupDto>> Create([FromBody] CreateStudyGroupCommand command, CancellationToken cancellationToken)
    {
        StudyGroupDto studyGroup = await sender.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = studyGroup.Id }, studyGroup);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<StudyGroupDto>> Update(int id, [FromBody] UpdateStudyGroupRequest request, CancellationToken cancellationToken)
    {
        UpdateStudyGroupCommand command = new UpdateStudyGroupCommand(
            id,
            request.Name,
            request.SuborganizationId,
            request.StudyProgramId,
            request.StudyPlanId);

        StudyGroupDto studyGroup = await sender.Send(command, cancellationToken);

        return Ok(studyGroup);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteStudyGroupCommand(id), cancellationToken);

        return NoContent();
    }
}

public sealed record UpdateStudyGroupRequest(
    string Name,
    int SuborganizationId,
    int? StudyProgramId,
    int? StudyPlanId);