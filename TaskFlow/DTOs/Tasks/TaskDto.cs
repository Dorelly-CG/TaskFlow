namespace TaskFlow.DTOs.Tasks
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public int ResponsibleUserId { get; set; }
        public string ResponsibleUserName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public byte TaskPriorityId { get; set; }
        public string TaskPriorityName { get; set; } = null!;
        public byte TaskStatusId { get; set; }
        public string TaskStatusName { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
