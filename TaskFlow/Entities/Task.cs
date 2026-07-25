using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Entities;

[Table("Tasks", Schema = "TF")]
[Index("ResponsibleUserId", Name = "IX_Tasks_ResponsibleUserID")]
[Index("TaskPriorityId", Name = "IX_Tasks_TaskPriorityID")]
[Index("TaskStatusId", Name = "IX_Tasks_TaskStatusID")]
public partial class Task
{
    [Key]
    [Column("TaskID")]
    public int TaskId { get; set; }

    [Column("ResponsibleUserID")]
    public int ResponsibleUserId { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [Column("TaskPriorityID")]
    public byte TaskPriorityId { get; set; }

    [Column("TaskStatusID")]
    public byte TaskStatusId { get; set; }

    [Precision(0)]
    public DateTime? StartDate { get; set; }

    [Precision(0)]
    public DateTime? CompletionDate { get; set; }

    [Precision(0)]
    public DateTime DueDate { get; set; }

    [Precision(0)]
    public DateTime AddedAt { get; set; }

    public bool IsDeleted { get; set; }

    [Precision(0)]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("ResponsibleUserId")]
    [InverseProperty("Tasks")]
    public virtual User ResponsibleUser { get; set; } = null!;

    [ForeignKey("TaskPriorityId")]
    [InverseProperty("Tasks")]
    public virtual TaskPriority TaskPriority { get; set; } = null!;

    [ForeignKey("TaskStatusId")]
    [InverseProperty("Tasks")]
    public virtual TaskStatus TaskStatus { get; set; } = null!;

    [InverseProperty("Task")]
    public virtual ICollection<TaskStatusAudit> TaskStatusAudits { get; set; } = new List<TaskStatusAudit>();
}
