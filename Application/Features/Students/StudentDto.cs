using Domain.Entities;

namespace Application.Features.Students;

public sealed record StudentDto(
    int Id,
    string Email,
    string FirstName,
    string LastName,
    string? Patronymic,
    int? AdmissionBase,
    int? AdmissionYear)
{
    public static StudentDto From(Student student)
    {
        return new StudentDto(
            student.Id,
            student.Email,
            student.FirstName,
            student.LastName,
            student.Patronymic,
            student.AdmissionBase,
            student.AdmissionYear
        );
    }
}