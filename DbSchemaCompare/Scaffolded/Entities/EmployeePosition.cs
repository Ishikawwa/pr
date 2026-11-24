using System;
using System.Collections.Generic;

namespace DbSchemaCompare.Scaffolded.Entities;

public partial class EmployeePosition
{
    public int EmployeeId { get; set; }

    public int Id { get; set; }

    public int PositionId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Position Position { get; set; } = null!;
}
