namespace WebApi.Dtos.Disciplines;

public sealed record PatchDisciplineDto(string? Code, string? Name, int? SuborganizationId);
