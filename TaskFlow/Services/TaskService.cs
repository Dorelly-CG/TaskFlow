using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.DTOs.Tasks;
using TaskFlow.Exceptions;
using TaskFlow.Interfaces.Repositories;
using TaskFlow.Interfaces.Services;

namespace TaskFlow.Services
{
    public class TaskService : ITaskService
    {


        private readonly ITaskRepository _taskRepository;
        private readonly TaskFlowDbContext _context;

        public TaskService(
            ITaskRepository taskRepository,
            TaskFlowDbContext context)
        {
            _taskRepository = taskRepository;
            _context = context;
        }

        public async Task<PageResultDto<TaskDto>> GetAllAsync(
            TaskFilterDto filter)
        {
            ValidatePagination(filter);

            IQueryable<Entities.Task> query = _taskRepository
                .GetQueryable()
                .Include(task => task.ResponsibleUser)
                .Include(task => task.TaskPriority)
                .Include(task => task.TaskStatus);

            if (filter.PriorityId.HasValue)
            {
                query = query.Where(task =>
                    task.TaskPriorityId == filter.PriorityId.Value);
            }

            if (filter.StatusId.HasValue)
            {
                query = query.Where(task =>
                    task.TaskStatusId == filter.StatusId.Value);
            }

            if (filter.UserId.HasValue)
            {
                query = query.Where(task =>
                    task.ResponsibleUserId == filter.UserId.Value);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(task =>
                    task.DueDate >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(task =>
                    task.DueDate <= filter.EndDate.Value);
            }

            if (filter.StartDate.HasValue &&
                filter.EndDate.HasValue &&
                filter.StartDate.Value > filter.EndDate.Value)
            {
                throw new BusinessException(
                    "La fecha inicial no puede ser mayor que la fecha final.");
            }

            int totalItems = await query.CountAsync();

            List<TaskDto> items = await query
                .OrderBy(task => task.DueDate)
                .ThenBy(task => task.TaskId)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(task => new TaskDto
                {
                    TaskId = task.TaskId,
                    ResponsibleUserId = task.ResponsibleUserId,
                    ResponsibleUserName = task.ResponsibleUser.FullName,
                    Title = task.Title,
                    Description = task.Description,
                    TaskPriorityId = task.TaskPriorityId,
                    TaskPriorityName = task.TaskPriority.Name,
                    TaskStatusId = task.TaskStatusId,
                    TaskStatusName = task.TaskStatus.Name,
                    StartDate = task.StartDate,
                    CompletionDate = task.CompletionDate,
                    DueDate = task.DueDate,
                    AddedAt = task.AddedAt
                })
                .ToListAsync();

            return new PageResultDto<TaskDto>
            {
                Items = items,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(
                    totalItems / (double)filter.PageSize)
            };
        }

        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            return await _taskRepository
                .GetQueryable()
                .Where(task => task.TaskId == id)
                .Select(task => new TaskDto
                {
                    TaskId = task.TaskId,
                    ResponsibleUserId = task.ResponsibleUserId,
                    ResponsibleUserName = task.ResponsibleUser.FullName,
                    Title = task.Title,
                    Description = task.Description,
                    TaskPriorityId = task.TaskPriorityId,
                    TaskPriorityName = task.TaskPriority.Name,
                    TaskStatusId = task.TaskStatusId,
                    TaskStatusName = task.TaskStatus.Name,
                    StartDate = task.StartDate,
                    CompletionDate = task.CompletionDate,
                    DueDate = task.DueDate,
                    AddedAt = task.AddedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            await ValidateTaskAsync(
                dto.ResponsibleUserId,
                dto.Title,
                dto.Description,
                dto.TaskPriorityId,
                null,
                dto.DueDate);

            var task = new Entities.Task
            {
                ResponsibleUserId = dto.ResponsibleUserId,
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                TaskPriorityId = dto.TaskPriorityId,

                // Pending
                TaskStatusId = 1,

                DueDate = dto.DueDate
            };

            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();

            return (await GetByIdAsync(task.TaskId))!;
        }

        public async Task<TaskDto?> UpdateAsync(
            int id,
            UpdateTaskDto dto)
        {
            Entities.Task? task = await _taskRepository.GetByIdAsync(id);

            if (task is null)
            {
                return null;
            }

            await ValidateTaskAsync(
                dto.ResponsibleUserId,
                dto.Title,
                dto.Description,
                dto.TaskPriorityId,
                id,
                dto.DueDate);

            bool statusExists = await _context.TaskStatuses.AnyAsync(status =>
                status.TaskStatusId == dto.TaskStatusId &&
                status.IsActive);

            if (!statusExists)
            {
                throw new BusinessException(
                    "El estatus indicado no existe o está inactivo.");
            }

            task.ResponsibleUserId = dto.ResponsibleUserId;
            task.Title = dto.Title.Trim();
            task.Description = dto.Description?.Trim();
            task.TaskPriorityId = dto.TaskPriorityId;
            task.TaskStatusId = dto.TaskStatusId;
            task.StartDate = dto.StartDate;
            task.CompletionDate = dto.CompletionDate;
            task.DueDate = dto.DueDate;

            await _taskRepository.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Entities.Task? task = await _taskRepository.GetByIdAsync(id);

            if (task is null)
            {
                return false;
            }

            task.IsDeleted = true;
            task.DeletedAt = DateTime.UtcNow;

            await _taskRepository.SaveChangesAsync();

            return true;
        }

        private async Task ValidateTaskAsync(
            int responsibleUserId,
            string title,
            string? description,
            byte priorityId,
            int? excludedTaskId,
            DateTime dueDate)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new BusinessException(
                    "El título es obligatorio.");
            }

            if (title.Trim().Length > 200)
            {
                throw new BusinessException(
                    "El título no puede exceder 200 caracteres.");
            }

            if (description?.Length > 500)
            {
                throw new BusinessException(
                    "La descripción no puede exceder 500 caracteres.");
            }

            if (dueDate.Date < DateTime.UtcNow.Date)
            {
                throw new BusinessException(
                    "La fecha límite no puede ser menor a hoy.");
            }

            bool userExists = await _context.Users.AnyAsync(user =>
                user.UserId == responsibleUserId &&
                user.IsActive);

            if (!userExists)
            {
                throw new BusinessException(
                    "El usuario responsable no existe o está inactivo.");
            }

            bool priorityExists = await _context.TaskPriorities.AnyAsync(priority =>
                priority.TaskPriorityId == priorityId &&
                priority.IsActive);

            if (!priorityExists)
            {
                throw new BusinessException(
                    "La prioridad no existe o está inactiva.");
            }

            bool duplicateExists =
                await _taskRepository.ExistsDuplicateTitleAsync(
                    responsibleUserId,
                    title.Trim(),
                    excludedTaskId);

            if (duplicateExists)
            {
                throw new BusinessException(
                    "El usuario ya tiene una tarea con el mismo título.");
            }
        }

        private static void ValidatePagination(TaskFilterDto filter)
        {
            if (filter.Page < 1)
            {
                throw new BusinessException(
                    "La página debe ser mayor o igual a 1.");
            }

            if (filter.PageSize < 1)
            {
                throw new BusinessException(
                    "El tamaño de página debe ser mayor o igual a 1.");
            }

            if (filter.PageSize > 100)
            {
                throw new BusinessException(
                    "El tamaño de página no puede exceder 100.");
            }
        }

    }
}
