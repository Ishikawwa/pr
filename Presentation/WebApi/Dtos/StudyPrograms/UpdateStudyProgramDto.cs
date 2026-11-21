namespace WebApi.Dtos.StudyPrograms;

public sealed record UpdateStudyProgramDto(string Name, string Qualification, string SpecialtyCode, int SuborganizationId);
