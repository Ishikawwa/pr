using System;
using System.Collections.Generic;

namespace DbSchemaCompare.Scaffolded.Entities;

public partial class Organization
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Suborganization> Suborganizations { get; set; } = new List<Suborganization>();
}
