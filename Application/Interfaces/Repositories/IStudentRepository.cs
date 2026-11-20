using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<IReadOnlyList<Student>> GetAllAsync(CancellationToken cancellationToken);

    Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<bool> EmailExistsAsync(string email, int? excludingId, CancellationToken cancellationToken);

    Task<Student> AddAsync(Student student, CancellationToken cancellationToken);

    Task UpdateAsync(Student student, CancellationToken cancellationToken);

    Task DeleteAsync(Student student, CancellationToken cancellationToken);
}