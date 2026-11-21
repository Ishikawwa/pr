namespace WebApi.Dtos.StudyPrograms;

public sealed record CreateStudyProgramDto(string Name, string Qualification, string SpecialtyCode, int SuborganizationId);
