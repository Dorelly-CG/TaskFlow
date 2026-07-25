namespace TaskFlow.DTOs.Reports
{
    public class PendingTaskReportDto
    {
        public string Usuario { get; set; } = null!;
        public int TotalPendientes { get; set; }
        public int TotalVencidas { get; set; }
    }
}
