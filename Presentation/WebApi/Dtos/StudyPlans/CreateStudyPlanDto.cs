namespace WebApi.Dtos.StudyPlans;

public sealed record CreateStudyPlanDto(int AdmissionBase, int AdmissionYear, string? Name, string? SourceFileName, int StudyProgramId);
