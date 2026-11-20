using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Student
{
    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public int? AdmissionBase { get; set; }

    public int? AdmissionYear { get; set; }

    public virtual ICollection<StudentDisciplineGrade> StudentDisciplineGrades { get; set; } = new List<StudentDisciplineGrade>();

    public virtual ICollection<StudentLicense> StudentLicenses { get; set; } = new List<StudentLicense>();
}
