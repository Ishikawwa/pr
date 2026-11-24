using System;
using System.Collections.Generic;

namespace DbSchemaCompare.Scaffolded.Entities;

public partial class AttestationCommission
{
    public int? DisciplineId { get; set; }

    public int Id { get; set; }

    public int? StudyGroupId { get; set; }

    public int StudyProgramId { get; set; }

    public string? AcademicYear { get; set; }

    public string? SemesterLabel { get; set; }

    public int? SemesterNumber { get; set; }

    public virtual ICollection<AttestationCommissionMember> AttestationCommissionMembers { get; set; } = new List<AttestationCommissionMember>();

    public virtual Discipline? Discipline { get; set; }

    public virtual StudyGroup? StudyGroup { get; set; }

    public virtual StudyProgram StudyProgram { get; set; } = null!;
}
