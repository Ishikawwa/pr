namespace WebApi.Dtos.AttestationCommissions;

public sealed record PatchAttestationCommissionDto(int? DisciplineId, int? StudyGroupId, int? StudyProgramId, string? AcademicYear, string? SemesterLabel, int? SemesterNumber);