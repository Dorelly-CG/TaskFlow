namespace TaskFlow.DTOs.Tasks
{
    public class TaskFilterDto
    {
        public byte? PriorityId { get; set; }
        public byte? StatusId { get; set; }
        public int? UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
