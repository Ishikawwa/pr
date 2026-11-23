using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class StudyPlan
{
    public int AdmissionBase { get; set; }

    public int AdmissionYear { get; set; }

    public int Id { get; set; }

    public string? Name { get; set; }

    public string? SourceFileName { get; set; }

    public int StudyProgramId { get; set; }

    public virtual ICollection<StudyGroup> StudyGroups { get; set; } = new List<StudyGroup>();

    public virtual ICollection<StudyPlanDiscipline> StudyPlanDisciplines { get; set; } = new List<StudyPlanDiscipline>();

    public virtual StudyProgram StudyProgram { get; set; } = null!;
}
