
using Application.Behaviour.StudyPrograms;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.StudyPrograms;

namespace WebApi.Controllers;

[ApiController]
[Route("api/studyprograms")]
public sealed class StudyProgramsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StudyProgramDto>>> GetAll([FromQuery] string? name, [FromQuery] string? specialtyCode, [FromQuery] int? suborganizationId, CancellationToken cancellationToken)
    {
        IReadOnlyList<StudyProgram> studyPrograms = await sender.Send(new GetStudyProgramsQuery(name, specialtyCode, suborganizationId), cancellationToken);

        return Ok(studyPrograms.Select(studyProgram => new StudyProgramDto(studyProgram.Id, studyProgram.Name, studyProgram.Qualification, studyProgram.SpecialtyCode, studyProgram.SuborganizationId)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudyProgramDto>> GetById(int id, CancellationToken cancellationToken)
    {
        StudyProgram? studyProgram = await sender.Send(new GetStudyProgramByIdQuery(id), cancellationToken);

        if (studyProgram is null)
            return NotFound();

        return Ok(new StudyProgramDto(studyProgram.Id, studyProgram.Name, studyProgram.Qualification, studyProgram.SpecialtyCode, studyProgram.SuborganizationId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStudyProgramDto dto, CancellationToken cancellationToken)
    {
        int id = await sender.Send(new CreateStudyProgramCommand(dto.Name, dto.Qualification, dto.SpecialtyCode, dto.SuborganizationId), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateStudyProgramDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(new UpdateStudyProgramCommand(id, dto.Name, dto.Qualification, dto.SpecialtyCode, dto.SuborganizationId), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchStudyProgramDto dto, CancellationToken cancellationToken)
    {
        bool updated = await sender.Send(new PatchStudyProgramCommand(id, dto.Name, dto.Qualification, dto.SpecialtyCode, dto.SuborganizationId), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            bool deleted = await sender.Send(new DeleteStudyProgramCommand(id), cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Невозможно удалить образовательную программу: существуют связанные записи.");
        }
    }
}
