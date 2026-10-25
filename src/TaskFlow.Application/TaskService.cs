using TaskFlow.Domain;

namespace TaskFlow.Application;

public class TaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(string title)
    {
        var task = new TaskItem(title);
        await _repository.AddAsync(task);
        await _repository.SaveChangesAsync();
        return task.Id;
    }

    public async Task CompleteAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("تسک پیدا نشد");

        task.Complete();
        await _repository.SaveChangesAsync();
    }

    public Task<List<TaskItem>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }
}