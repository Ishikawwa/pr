using System;
using System.Collections.Generic;

namespace DbSchemaCompare.Scaffolded.Entities;

public partial class ImportBatch
{
    public int Id { get; set; }

    public DateTime ImportedAt { get; set; }

    public int RowCount { get; set; }

    public string? Semester { get; set; }

    public string SourceFileName { get; set; } = null!;
}
