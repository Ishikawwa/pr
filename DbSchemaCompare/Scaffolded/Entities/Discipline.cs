using System;
using System.Collections.Generic;

namespace DbSchemaCompare.Scaffolded.Entities;

public partial class Discipline
{
    public string Code { get; set; } = null!;

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int SuborganizationId { get; set; }

    public virtual ICollection<AttestationCommission> AttestationCommissions { get; set; } = new List<AttestationCommission>();

    public virtual ICollection<FgosDisciplineMapping> FgosDisciplineMappings { get; set; } = new List<FgosDisciplineMapping>();

    public virtual ICollection<StudentDisciplineGrade> StudentDisciplineGrades { get; set; } = new List<StudentDisciplineGrade>();

    public virtual Suborganization Suborganization { get; set; } = null!;
}
