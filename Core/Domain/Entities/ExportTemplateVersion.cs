using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ExportTemplateVersion
{
    public int? BasedOnVersionId { get; set; }

    public string ContentBase64 { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string Format { get; set; } = null!;

    public int Id { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public string OriginalFileName { get; set; } = null!;

    public int? StudyGroupId { get; set; }

    public string TemplateKey { get; set; } = null!;

    public int VersionNumber { get; set; }

    public virtual ExportTemplateVersion? BasedOnVersion { get; set; }

    public virtual ICollection<ExportTemplateVersion> InverseBasedOnVersion { get; set; } = new List<ExportTemplateVersion>();

    public virtual StudyGroup? StudyGroup { get; set; }
}
