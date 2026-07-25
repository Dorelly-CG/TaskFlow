using TaskFlow.DTOs.Reports;

namespace TaskFlow.Interfaces.Services
{
    public interface IReportService
    {
        Task<IEnumerable<PendingTaskReportDto>> GetPendingTasksAsync();
    }
}
