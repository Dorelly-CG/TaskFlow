using Microsoft.AspNetCore.Mvc;
using TaskFlow.DTOs.Tasks;
using TaskFlow.Interfaces.Services;

namespace TaskFlow.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<PageResultDto<TaskDto>>> GetAll(
            [FromQuery] TaskFilterDto filter)
        {
            PageResultDto<TaskDto> result =
                await _taskService.GetAllAsync(filter);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskDto>> GetById(int id)
        {
            TaskDto? task = await _taskService.GetByIdAsync(id);

            if (task is null)
            {
                return NotFound(new
                {
                    message = "La tarea no fue encontrada."
                });
            }

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> Create(
            [FromBody] CreateTaskDto dto)
        {
            TaskDto createdTask =
                await _taskService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdTask.TaskId },
                createdTask);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TaskDto>> Update(
            int id,
            [FromBody] UpdateTaskDto dto)
        {
            TaskDto? updatedTask =
                await _taskService.UpdateAsync(id, dto);

            if (updatedTask is null)
            {
                return NotFound(new
                {
                    message = "La tarea no fue encontrada."
                });
            }

            return Ok(updatedTask);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _taskService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "La tarea no fue encontrada."
                });
            }

            return Ok(new
            {
                message = "La tarea fue eliminada correctamente."
            });
        }
    }
}
