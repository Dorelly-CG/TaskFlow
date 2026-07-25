namespace TaskFlow.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        IQueryable<Entities.Task> GetQueryable();
        System.Threading.Tasks.Task<Entities.Task?> GetByIdAsync(int id);

        System.Threading.Tasks.Task<bool> ExistsDuplicateTitleAsync(
            int userId,
            string title,
            int? excludedTaskId = null);

        System.Threading.Tasks.Task AddAsync(Entities.Task task);

        System.Threading.Tasks.Task SaveChangesAsync();
    }
}
