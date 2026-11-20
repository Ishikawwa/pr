using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class AcademicPerformanceRecord
{
    public string? AllDisciplineTeachers { get; set; }

    public int AvailableControlPoints { get; set; }

    public string? CurrentDisciplinePercent { get; set; }

    public int CurrentPointsFivePointScale { get; set; }

    public string? CurrentStudentAveragePercent { get; set; }

    public int FinalPoints { get; set; }

    public int FinalPointsFivePointScale { get; set; }

    public int Id { get; set; }

    public int ImportBatchId { get; set; }

    public int MaxAvailableControlPoints { get; set; }

    public int MaxDisciplinePoints { get; set; }

    public int MaxPointsByTopics { get; set; }

    public int? RetakePoints { get; set; }

    public int? RetakePointsNew { get; set; }

    public int SourceRowNumber { get; set; }

    public int TaskAndTestPoints { get; set; }

    public string? TeachersFromWorkload { get; set; }

    public string? TeachersWhoGraded { get; set; }

    public int DisciplineId { get; set; }

    public int OrganizationId { get; set; }

    public int SemesterId { get; set; }

    public int StudentId { get; set; }

    public int StudyGroupId { get; set; }

    public int SuborganizationId { get; set; }

    public virtual ICollection<StudentDisciplineGrade> StudentDisciplineGrades { get; set; } = new List<StudentDisciplineGrade>();
}
