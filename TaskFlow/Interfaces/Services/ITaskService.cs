using TaskFlow.DTOs.Tasks;

namespace TaskFlow.Interfaces.Services
{
    public interface ITaskService
    {
        Task<PageResultDto<TaskDto>> GetAllAsync(TaskFilterDto filter);

        Task<TaskDto?> GetByIdAsync(int id);

        Task<TaskDto> CreateAsync(CreateTaskDto dto);

        Task<TaskDto?> UpdateAsync(int id, UpdateTaskDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
