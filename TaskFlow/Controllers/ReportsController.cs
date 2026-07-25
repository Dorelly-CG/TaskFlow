using Microsoft.AspNetCore.Mvc;
using TaskFlow.DTOs.Reports;
using TaskFlow.Interfaces.Services;

namespace TaskFlow.Controllers
{
    [ApiController]
    [Route("reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("pending-tasks")]
        [ProducesResponseType(
            typeof(IEnumerable<PendingTaskReportDto>),
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PendingTaskReportDto>>>
            GetPendingTasks()
        {
            IEnumerable<PendingTaskReportDto> result =
                await _reportService.GetPendingTasksAsync();

            return Ok(result);
        }
    }
}
