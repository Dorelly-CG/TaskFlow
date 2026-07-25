using System.ComponentModel.DataAnnotations;

namespace TaskFlow.DTOs.Tasks
{
    public class CreateTaskDto
    {
        public int ResponsibleUserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        public byte TaskPriorityId { get; set; }

        public DateTime DueDate { get; set; }
    }
}
