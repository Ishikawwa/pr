namespace WebApi.Dtos.StudentLicenseHistories;

public sealed record CreateStudentLicenseHistoryDto(int StudentLicenseId, string? AcademicYear, int? SemesterNumber, string SemesterLabel);
