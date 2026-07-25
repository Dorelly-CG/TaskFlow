using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Entities;

[Table("TaskPriority", Schema = "TF")]
[Index("Name", Name = "UQ_TaskPriority_Name", IsUnique = true)]
public partial class TaskPriority
{
    [Key]
    [Column("TaskPriorityID")]
    public byte TaskPriorityId { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    [InverseProperty("TaskPriority")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
