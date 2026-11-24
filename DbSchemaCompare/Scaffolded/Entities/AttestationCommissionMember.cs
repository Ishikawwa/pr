using System;
using System.Collections.Generic;

namespace DbSchemaCompare.Scaffolded.Entities;

public partial class AttestationCommissionMember
{
    public int CommissionId { get; set; }

    public string FullName { get; set; } = null!;

    public int Id { get; set; }

    public string? Position { get; set; }

    public string Role { get; set; } = null!;

    public int SortOrder { get; set; }

    public virtual AttestationCommission Commission { get; set; } = null!;
}
