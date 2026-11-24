namespace WebApi.Dtos.StudyPlans;

public sealed record PatchStudyPlanDto(int? AdmissionBase = null, int? AdmissionYear = null, string? Name = null, string? SourceFileName = null, int? StudyProgramId = null);
