namespace WebApi.Dtos.StudyPlanDisciplines;

public sealed record UpdateStudyPlanDisciplineDto(int StudyPlanId, int FgosDisciplineId, int SemesterNumber, int? PlannedHours, string? ControlForm);
