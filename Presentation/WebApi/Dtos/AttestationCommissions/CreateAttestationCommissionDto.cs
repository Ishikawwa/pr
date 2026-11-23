namespace WebApi.Dtos.AttestationCommissions;

public sealed record CreateAttestationCommissionDto(int? DisciplineId, int? StudyGroupId, int StudyProgramId, string? AcademicYear, string? SemesterLabel, int? SemesterNumber);