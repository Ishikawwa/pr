namespace WebApi.Dtos.StudentLicenseHistories;

public sealed record StudentLicenseHistoryDto(int Id, int StudentLicenseId, int StudentId, string StudentFullName, int LicenseId, string LicenseName, string? AcademicYear, int? SemesterNumber, string SemesterLabel);
