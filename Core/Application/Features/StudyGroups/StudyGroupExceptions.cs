namespace Application.Features.StudyGroups;

public sealed class StudyGroupNotFoundException(int id)
    : Exception($"Study group with ID {id} was not found.");

public sealed class StudyGroupNameConflictException(string name, int suborganizationId)
    : Exception($"Study group '{name}' already exists in suborganization {suborganizationId}.");