namespace WebApi.Dtos.StudyPlans;

public sealed record StudyPlanDto(int Id, int AdmissionBase, int AdmissionYear, string? Name, string? SourceFileName, int StudyProgramId);
