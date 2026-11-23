using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class FgosDiscipline
{
    public string Code { get; set; } = null!;

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int StudyProgramId { get; set; }

    public virtual ICollection<FgosDisciplineMapping> FgosDisciplineMappings { get; set; } = new List<FgosDisciplineMapping>();

    public virtual ICollection<StudyPlanDiscipline> StudyPlanDisciplines { get; set; } = new List<StudyPlanDiscipline>();

    public virtual StudyProgram StudyProgram { get; set; } = null!;
}
