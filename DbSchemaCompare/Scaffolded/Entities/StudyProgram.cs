using System;
using System.Collections.Generic;

namespace DbSchemaCompare.Scaffolded.Entities;

public partial class StudyProgram
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Qualification { get; set; } = null!;

    public string SpecialtyCode { get; set; } = null!;

    public int SuborganizationId { get; set; }

    public virtual ICollection<AttestationCommission> AttestationCommissions { get; set; } = new List<AttestationCommission>();

    public virtual ICollection<FgosDiscipline> FgosDisciplines { get; set; } = new List<FgosDiscipline>();

    public virtual ICollection<StudyGroup> StudyGroups { get; set; } = new List<StudyGroup>();

    public virtual ICollection<StudyPlan> StudyPlans { get; set; } = new List<StudyPlan>();

    public virtual Suborganization Suborganization { get; set; } = null!;
}
