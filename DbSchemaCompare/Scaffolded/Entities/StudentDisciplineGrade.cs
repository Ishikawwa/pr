using System;
using System.Collections.Generic;

namespace DbSchemaCompare.Scaffolded.Entities;

public partial class StudentDisciplineGrade
{
    public int? AcademicPerformanceId { get; set; }

    public int AdmissionYear { get; set; }

    public DateTime Date { get; set; }

    public int DisciplineId { get; set; }

    public int EmployeeId { get; set; }

    public string Grade { get; set; } = null!;

    public int Id { get; set; }

    public int SemesterNumber { get; set; }

    public int StudentId { get; set; }

    public virtual AcademicPerformanceRecord? AcademicPerformance { get; set; }

    public virtual Discipline Discipline { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
