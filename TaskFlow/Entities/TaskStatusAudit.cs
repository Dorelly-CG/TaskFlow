using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Entities;

[Table("TaskStatusAudit", Schema = "TF")]
[Index("TaskId", Name = "IX_TaskStatusAudit_TaskID")]
public partial class TaskStatusAudit
{
    [Key]
    [Column("TaskStatusAuditID")]
    public long TaskStatusAuditId { get; set; }

    [Column("TaskID")]
    public int TaskId { get; set; }

    [Column("ChangedByUserID")]
    public int? ChangedByUserId { get; set; }

    [Column("PreviousStatusID")]
    public byte PreviousStatusId { get; set; }

    [Column("NewStatusID")]
    public byte NewStatusId { get; set; }

    [Precision(0)]
    public DateTime ChangedAt { get; set; }

    [ForeignKey("ChangedByUserId")]
    [InverseProperty("TaskStatusAudits")]
    public virtual User? ChangedByUser { get; set; }

    [ForeignKey("NewStatusId")]
    [InverseProperty("TaskStatusAuditNewStatuses")]
    public virtual TaskStatus NewStatus { get; set; } = null!;

    [ForeignKey("PreviousStatusId")]
    [InverseProperty("TaskStatusAuditPreviousStatuses")]
    public virtual TaskStatus PreviousStatus { get; set; } = null!;

    [ForeignKey("TaskId")]
    [InverseProperty("TaskStatusAudits")]
    public virtual Task Task { get; set; } = null!;
}
