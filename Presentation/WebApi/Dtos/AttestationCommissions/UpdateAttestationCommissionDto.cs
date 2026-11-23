namespace WebApi.Dtos.AttestationCommissions;

public sealed record UpdateAttestationCommissionDto(int? DisciplineId, int? StudyGroupId, int StudyProgramId, string? AcademicYear, string? SemesterLabel, int? SemesterNumber);