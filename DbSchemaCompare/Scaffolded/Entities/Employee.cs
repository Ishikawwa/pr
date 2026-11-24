using System;
using System.Collections.Generic;

namespace DbSchemaCompare.Scaffolded.Entities;

public partial class Employee
{
    public string FullName { get; set; } = null!;

    public int Id { get; set; }

    public virtual ICollection<EmployeePosition> EmployeePositions { get; set; } = new List<EmployeePosition>();

    public virtual ICollection<StudentDisciplineGrade> StudentDisciplineGrades { get; set; } = new List<StudentDisciplineGrade>();
}
