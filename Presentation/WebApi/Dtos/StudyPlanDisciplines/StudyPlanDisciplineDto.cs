namespace WebApi.Dtos.StudyPlanDisciplines;

public sealed record StudyPlanDisciplineDto(int Id, int StudyPlanId, string? StudyPlanName, int FgosDisciplineId, string FgosDisciplineCode, string FgosDisciplineName, int SemesterNumber, int? PlannedHours, string? ControlForm);
