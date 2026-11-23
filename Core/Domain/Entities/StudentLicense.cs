using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class StudentLicense
{
    public int Id { get; set; }

    public int LicenseId { get; set; }

    public int StudentId { get; set; }

    public virtual License License { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual ICollection<StudentLicenseHistory> StudentLicenseHistories { get; set; } = new List<StudentLicenseHistory>();
}
