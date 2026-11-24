using System;
using System.Collections.Generic;

namespace DbSchemaCompare.Scaffolded.Entities;

public partial class StudentLicenseHistory
{
    public string? AcademicYear { get; set; }

    public int Id { get; set; }

    public string SemesterLabel { get; set; } = null!;

    public int? SemesterNumber { get; set; }

    public int StudentLicenseId { get; set; }

    public virtual StudentLicense StudentLicense { get; set; } = null!;
}
