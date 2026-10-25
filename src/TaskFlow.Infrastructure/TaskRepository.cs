using Microsoft.EntityFrameworkCore;
using TaskFlow.Application;
using TaskFlow.Domain;

namespace TaskFlow.Infrastructure;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;

    public TaskRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(TaskItem task) => await _db.Tasks.AddAsync(task);

    public Task<TaskItem?> GetByIdAsync(Guid id) =>
        _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

    public Task<List<TaskItem>> GetAllAsync() => _db.Tasks.ToListAsync();

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}