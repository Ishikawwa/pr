namespace Application.Features.StudentDisciplineGrades;

public sealed class StudentDisciplineGradeNotFoundException(int id)
    : Exception($"Student discipline grade with ID {id} was not found.");