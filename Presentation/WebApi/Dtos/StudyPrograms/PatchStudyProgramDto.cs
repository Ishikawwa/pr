
namespace WebApi.Dtos.StudyPrograms;

public sealed record PatchStudyProgramDto(string? Name, string? Qualification, string? SpecialtyCode, int? SuborganizationId);
