using NSubstitute;
using TaskFlow.Application;
using TaskFlow.Domain;

namespace TaskFlow.UnitTests.Application;

public class TaskServiceTests
{
    private readonly ITaskRepository _repository;
    private readonly TaskService _sut;

    public TaskServiceTests()
    {
        _repository = Substitute.For<ITaskRepository>();
        _sut = new TaskService(_repository);
    }

    // ---------- CreateAsync ----------

    [Fact]
    public async Task CreateAsync_WithValidTitle_AddsTaskAndSaves()
    {
        var id = await _sut.CreateAsync("خرید نان");

        await _repository.Received(1).AddAsync(
            Arg.Is<TaskItem>(t => t.Id == id && t.Title == "خرید نان" && !t.IsDone));
        await _repository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task CreateAsync_WithEmptyTitle_ThrowsAndDoesNotSave()
    {
        var act = () => _sut.CreateAsync("");

        await act.Should().ThrowAsync<ArgumentException>();
        await _repository.DidNotReceive().AddAsync(Arg.Any<TaskItem>());
        await _repository.DidNotReceive().SaveChangesAsync();
    }

    // ---------- CompleteAsync ----------

    [Fact]
    public async Task CompleteAsync_WhenTaskExists_MarksAsDoneAndSaves()
    {
        var task = new TaskItem("خرید نان");
        _repository.GetByIdAsync(task.Id).Returns(task);

        await _sut.CompleteAsync(task.Id);

        task.IsDone.Should().BeTrue();
        await _repository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task CompleteAsync_WhenTaskNotFound_ThrowsKeyNotFoundAndDoesNotSave()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id).Returns((TaskItem?)null);

        var act = () => _sut.CompleteAsync(id);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("تسک پیدا نشد");
        await _repository.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task CompleteAsync_WhenAlreadyDone_ThrowsInvalidOperationAndDoesNotSave()
    {
        var task = new TaskItem("خرید نان");
        task.Complete();
        _repository.GetByIdAsync(task.Id).Returns(task);

        var act = () => _sut.CompleteAsync(task.Id);

        await act.Should().ThrowAsync<InvalidOperationException>();
        await _repository.DidNotReceive().SaveChangesAsync();
    }

    // ---------- GetAllAsync ----------

    [Fact]
    public async Task GetAllAsync_ReturnsTasksFromRepository()
    {
        var tasks = new List<TaskItem> { new("اول"), new("دوم") };
        _repository.GetAllAsync().Returns(tasks);

        var result = await _sut.GetAllAsync();

        result.Should().BeSameAs(tasks);
    }
}