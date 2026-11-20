using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class StudyPlanDiscipline
{
    public string? ControlForm { get; set; }

    public int FgosDisciplineId { get; set; }

    public int Id { get; set; }

    public int? PlannedHours { get; set; }

    public int SemesterNumber { get; set; }

    public int StudyPlanId { get; set; }

    public virtual FgosDiscipline FgosDiscipline { get; set; } = null!;

    public virtual StudyPlan StudyPlan { get; set; } = null!;
}
