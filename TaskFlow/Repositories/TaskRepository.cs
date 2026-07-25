using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Interfaces.Repositories;

namespace TaskFlow.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskFlowDbContext _context;

        public TaskRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public IQueryable<Entities.Task> GetQueryable()
        {
            return _context.Tasks
                .AsNoTracking()
                .Where(task => !task.IsDeleted);
        }

        public async System.Threading.Tasks.Task<Entities.Task?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(task =>
                    task.TaskId == id &&
                    !task.IsDeleted);
        }

        public async System.Threading.Tasks.Task<bool> ExistsDuplicateTitleAsync(
            int userId,
            string title,
            int? excludedTaskId = null)
        {
            string normalizedTitle = title.Trim();

            return await _context.Tasks.AnyAsync(task =>
                !task.IsDeleted &&
                task.ResponsibleUserId == userId &&
                task.Title == normalizedTitle &&
                (!excludedTaskId.HasValue ||
                 task.TaskId != excludedTaskId.Value));
        }

        public async System.Threading.Tasks.Task AddAsync(Entities.Task task)
        {
            await _context.Tasks.AddAsync(task);
        }
        public async System.Threading.Tasks.Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
