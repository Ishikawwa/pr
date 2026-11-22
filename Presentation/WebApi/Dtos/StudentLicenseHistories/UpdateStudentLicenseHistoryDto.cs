namespace WebApi.Dtos.StudentLicenseHistories;

public sealed record UpdateStudentLicenseHistoryDto(int StudentLicenseId, string? AcademicYear, int? SemesterNumber, string SemesterLabel);
