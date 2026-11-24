namespace WebApi.Dtos.EmployeePositions;

public sealed record EmployeePositionDto(int Id, int EmployeeId, string EmployeeFullName, int PositionId, string PositionTitle);