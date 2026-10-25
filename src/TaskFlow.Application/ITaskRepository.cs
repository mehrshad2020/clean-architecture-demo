using TaskFlow.Domain;

namespace TaskFlow.Application;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<List<TaskItem>> GetAllAsync();
    Task SaveChangesAsync();
}