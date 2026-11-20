using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class StudyGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int SuborganizationId { get; set; }

    public int? StudyProgramId { get; set; }

    public int? StudyPlanId { get; set; }

    public virtual ICollection<AttestationCommission> AttestationCommissions { get; set; } = new List<AttestationCommission>();

    public virtual StudyPlan? StudyPlan { get; set; }

    public virtual StudyProgram? StudyProgram { get; set; }

    public virtual Suborganization Suborganization { get; set; } = null!;
}
