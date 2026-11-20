using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class FgosDisciplineMapping
{
    public int DisciplineId { get; set; }

    public int FgosDisciplineId { get; set; }

    public int Id { get; set; }

    public virtual Discipline Discipline { get; set; } = null!;

    public virtual FgosDiscipline FgosDiscipline { get; set; } = null!;
}
