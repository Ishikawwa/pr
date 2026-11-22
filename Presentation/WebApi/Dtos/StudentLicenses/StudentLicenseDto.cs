namespace WebApi.Dtos.StudentLicenses;

public sealed record StudentLicenseDto(int Id, int StudentId, string StudentFullName, int LicenseId, string LicenseName);
