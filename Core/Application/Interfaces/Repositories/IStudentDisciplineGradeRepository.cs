using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IStudentDisciplineGradeRepository
{
    Task<IReadOnlyList<StudentDisciplineGrade>> GetAllAsync(CancellationToken cancellationToken);

    Task<StudentDisciplineGrade?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<StudentDisciplineGrade> AddAsync(StudentDisciplineGrade studentDisciplineGrade, CancellationToken cancellationToken);

    Task UpdateAsync(StudentDisciplineGrade studentDisciplineGrade, CancellationToken cancellationToken);

    Task DeleteAsync(StudentDisciplineGrade studentDisciplineGrade, CancellationToken cancellationToken);
}