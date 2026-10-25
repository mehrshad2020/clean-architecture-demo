namespace TaskFlow.Domain;

public class TaskItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public bool IsDone { get; private set; }

    public TaskItem(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("عنوان خالی است");

        Id = Guid.NewGuid();
        Title = title;
        IsDone = false;
    }

    public void Complete()
    {
        if (IsDone)
            throw new InvalidOperationException("قبلاً انجام شده");

        IsDone = true;
    }
}