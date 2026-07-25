using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Entities;

[Table("TaskStatus", Schema = "TF")]
[Index("Name", Name = "UQ_TaskStatus_Name", IsUnique = true)]
public partial class TaskStatus
{
    [Key]
    [Column("TaskStatusID")]
    public byte TaskStatusId { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    [InverseProperty("NewStatus")]
    public virtual ICollection<TaskStatusAudit> TaskStatusAuditNewStatuses { get; set; } = new List<TaskStatusAudit>();

    [InverseProperty("PreviousStatus")]
    public virtual ICollection<TaskStatusAudit> TaskStatusAuditPreviousStatuses { get; set; } = new List<TaskStatusAudit>();

    [InverseProperty("TaskStatus")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
