using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Entities;

[Table("Users", Schema = "TF")]
[Index("Email", Name = "UQ_Users_Email", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(150)]
    public string FullName { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Precision(0)]
    public DateTime AddedAt { get; set; }

    public bool IsActive { get; set; }

    [Precision(0)]
    public DateTime? DeactivatedAt { get; set; }

    [InverseProperty("ChangedByUser")]
    public virtual ICollection<TaskStatusAudit> TaskStatusAudits { get; set; } = new List<TaskStatusAudit>();

    [InverseProperty("ResponsibleUser")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
