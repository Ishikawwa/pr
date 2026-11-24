using System;
using System.Collections.Generic;

namespace DbSchemaCompare.Scaffolded.Entities;

public partial class License
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<StudentLicense> StudentLicenses { get; set; } = new List<StudentLicense>();
}
