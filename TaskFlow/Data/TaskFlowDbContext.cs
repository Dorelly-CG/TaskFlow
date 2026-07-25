using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Entities;

namespace TaskFlow.Data;

public partial class TaskFlowDbContext : DbContext
{
    public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Entities.Task> Tasks { get; set; }

    public virtual DbSet<TaskPriority> TaskPriorities { get; set; }

    public virtual DbSet<Entities.TaskStatus> TaskStatuses { get; set; }

    public virtual DbSet<TaskStatusAudit> TaskStatusAudits { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Task>(entity =>
        {
            entity.ToTable("Tasks", "TF", tb => tb.HasTrigger("TR_Tasks_TaskStatusAudit"));

            entity.Property(e => e.AddedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.ResponsibleUser).WithMany(p => p.Tasks)
                //.OnDelete(DeleteBehavior.NoAction)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Users");

            entity.HasOne(d => d.TaskPriority).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_TaskPriority");

            entity.HasOne(d => d.TaskStatus).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_TaskStatus");
        });

        modelBuilder.Entity<TaskPriority>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Entities.TaskStatus>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<TaskStatusAudit>(entity =>
        {
            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.ChangedByUser).WithMany(p => p.TaskStatusAudits).HasConstraintName("FK_TaskStatusAudit_ChangedByUser");

            entity.HasOne(d => d.NewStatus).WithMany(p => p.TaskStatusAuditNewStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskStatusAudit_NewStatus");

            entity.HasOne(d => d.PreviousStatus).WithMany(p => p.TaskStatusAuditPreviousStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskStatusAudit_PreviousStatus");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskStatusAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskStatusAudit_Tasks");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.AddedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
