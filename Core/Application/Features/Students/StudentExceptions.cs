namespace Application.Features.Students;

public sealed class StudentNotFoundException(int id)
    : Exception($"Student with ID {id} was not found.");

public sealed class StudentEmailConflictException(string email)
    : Exception($"Student with email '{email}' already exists.");