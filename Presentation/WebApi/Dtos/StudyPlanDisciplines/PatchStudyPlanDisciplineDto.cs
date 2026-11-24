namespace WebApi.Dtos.StudyPlanDisciplines;

public sealed record PatchStudyPlanDisciplineDto(int? StudyPlanId, int? FgosDisciplineId, int? SemesterNumber, int? PlannedHours, string? ControlForm);
