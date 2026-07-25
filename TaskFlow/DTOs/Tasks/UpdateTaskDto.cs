using System.ComponentModel.DataAnnotations;

namespace TaskFlow.DTOs.Tasks
{
    public class UpdateTaskDto
    {
        public int ResponsibleUserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        public byte TaskPriorityId { get; set; }

        public byte TaskStatusId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
