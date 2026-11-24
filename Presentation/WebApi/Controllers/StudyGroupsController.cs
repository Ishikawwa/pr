
using System.Text.Json.Serialization;
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
    public async Task<ActionResult<IReadOnlyList<StudyGroupDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] int? suborganizationId,
        [FromQuery] int? studyProgramId,
        [FromQuery] int? studyPlanId,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<StudyGroupDto> studyGroups = await sender.Send(
            new GetStudyGroupsQuery(
                search,
                suborganizationId,
                studyProgramId,
                studyPlanId),
            cancellationToken);

        return Ok(studyGroups);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudyGroupDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        StudyGroupDto studyGroup = await sender.Send(
            new GetStudyGroupByIdQuery(id),
            cancellationToken);

        return Ok(studyGroup);
    }

    [HttpPost]
    public async Task<ActionResult<StudyGroupDto>> Create(
        [FromBody] CreateStudyGroupCommand command,
        CancellationToken cancellationToken)
    {
        StudyGroupDto studyGroup = await sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = studyGroup.Id },
            studyGroup);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<StudyGroupDto>> Update(
        int id,
        [FromBody] UpdateStudyGroupRequest request,
        CancellationToken cancellationToken)
    {
        UpdateStudyGroupCommand command = new UpdateStudyGroupCommand(
            id,
            request.Name,
            request.SuborganizationId,
            request.StudyProgramId,
            request.StudyPlanId);

        StudyGroupDto studyGroup = await sender.Send(
            command,
            cancellationToken);

        return Ok(studyGroup);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<StudyGroupDto>> Patch(
        int id,
        [FromBody] PatchStudyGroupRequest request,
        CancellationToken cancellationToken)
    {
        PatchStudyGroupCommand command = new PatchStudyGroupCommand(
            id,
            request.Name,
            request.SuborganizationId,
            request.StudyProgramId,
            request.StudyPlanId,
            request.HasName,
            request.HasSuborganizationId,
            request.HasStudyProgramId,
            request.HasStudyPlanId);

        StudyGroupDto studyGroup = await sender.Send(
            command,
            cancellationToken);

        return Ok(studyGroup);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new DeleteStudyGroupCommand(id),
            cancellationToken);

        return NoContent();
    }
}

public sealed record UpdateStudyGroupRequest(
    string Name,
    int SuborganizationId,
    int? StudyProgramId,
    int? StudyPlanId);

public sealed class PatchStudyGroupRequest
{
    private string? _name;
    private int? _suborganizationId;
    private int? _studyProgramId;
    private int? _studyPlanId;

    [JsonIgnore]
    public bool HasName { get; private set; }

    [JsonIgnore]
    public bool HasSuborganizationId { get; private set; }

    [JsonIgnore]
    public bool HasStudyProgramId { get; private set; }

    [JsonIgnore]
    public bool HasStudyPlanId { get; private set; }

    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            HasName = true;
        }
    }

    public int? SuborganizationId
    {
        get => _suborganizationId;
        set
        {
            _suborganizationId = value;
            HasSuborganizationId = true;
        }
    }

    public int? StudyProgramId
    {
        get => _studyProgramId;
        set
        {
            _studyProgramId = value;
            HasStudyProgramId = true;
        }
    }

    public int? StudyPlanId
    {
        get => _studyPlanId;
        set
        {
            _studyPlanId = value;
            HasStudyPlanId = true;
        }
    }
}
