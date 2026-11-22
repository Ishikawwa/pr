namespace WebApi.Dtos.Disciplines;

public sealed record CreateDisciplineDto(string Code, string Name, int SuborganizationId);
