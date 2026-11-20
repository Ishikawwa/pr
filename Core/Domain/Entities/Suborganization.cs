using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Suborganization
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int OrganizationId { get; set; }

    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();

    public virtual Organization Organization { get; set; } = null!;

    public virtual ICollection<StudyGroup> StudyGroups { get; set; } = new List<StudyGroup>();

    public virtual ICollection<StudyProgram> StudyPrograms { get; set; } = new List<StudyProgram>();
}
