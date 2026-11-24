using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ExportConfigurationVersion
{
    public string? ChangeNote { get; set; }

    public int ConfigurationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Settings { get; set; } = null!;

    public int VersionNumber { get; set; }

    public virtual ExportConfiguration Configuration { get; set; } = null!;

    public virtual ICollection<ExportConfiguration> ExportConfigurations { get; set; } = new List<ExportConfiguration>();
}
