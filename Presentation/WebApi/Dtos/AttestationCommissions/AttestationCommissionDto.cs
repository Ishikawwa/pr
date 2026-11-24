namespace WebApi.Dtos.AttestationCommissions;

public sealed record AttestationCommissionDto(int Id, int? DisciplineId, string? DisciplineName, int? StudyGroupId, string? StudyGroupName, int StudyProgramId, string StudyProgramName, string? AcademicYear, string? SemesterLabel, int? SemesterNumber);