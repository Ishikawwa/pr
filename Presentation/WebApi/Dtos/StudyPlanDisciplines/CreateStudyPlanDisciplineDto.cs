namespace WebApi.Dtos.StudyPlanDisciplines;

public sealed record CreateStudyPlanDisciplineDto(int StudyPlanId, int FgosDisciplineId, int SemesterNumber, int? PlannedHours, string? ControlForm);
