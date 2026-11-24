using System;
using System.Collections.Generic;

namespace DbSchemaCompare.Scaffolded.Entities;

public partial class StudyGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int SuborganizationId { get; set; }

    public int? StudyProgramId { get; set; }

    public int? StudyPlanId { get; set; }

    public virtual ICollection<AttestationCommission> AttestationCommissions { get; set; } = new List<AttestationCommission>();

    public virtual ICollection<ExportConfiguration> ExportConfigurations { get; set; } = new List<ExportConfiguration>();

    public virtual ICollection<ExportTemplateVersion> ExportTemplateVersions { get; set; } = new List<ExportTemplateVersion>();

    public virtual StudyPlan? StudyPlan { get; set; }

    public virtual StudyProgram? StudyProgram { get; set; }

    public virtual Suborganization Suborganization { get; set; } = null!;
}
