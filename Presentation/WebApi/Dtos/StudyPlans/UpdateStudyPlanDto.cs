
namespace WebApi.Dtos.StudyPlans;

public sealed record UpdateStudyPlanDto(int AdmissionBase, int AdmissionYear, string? Name, string? SourceFileName, int StudyProgramId);
