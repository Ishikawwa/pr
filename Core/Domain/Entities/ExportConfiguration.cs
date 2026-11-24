using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ExportConfiguration
{
    public int? ActiveVersionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int Id { get; set; }

    public bool IsActive { get; set; }

    public int? StudyGroupId { get; set; }

    public string TemplateKey { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public virtual ExportConfigurationVersion? ActiveVersion { get; set; }

    public virtual ICollection<ExportConfigurationVersion> ExportConfigurationVersions { get; set; } = new List<ExportConfigurationVersion>();

    public virtual StudyGroup? StudyGroup { get; set; }
}
