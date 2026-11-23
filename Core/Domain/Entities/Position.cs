using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Position
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<EmployeePosition> EmployeePositions { get; set; } = new List<EmployeePosition>();
}
