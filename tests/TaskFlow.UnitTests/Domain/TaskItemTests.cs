using TaskFlow.Domain;

namespace TaskFlow.UnitTests.Domain;

public class TaskItemTests
{
    [Fact]
    public void Constructor_WithValidTitle_CreatesTaskThatIsNotDone()
    {
        // Arrange
        var title = "خرید نان";

        // Act
        var task = new TaskItem(title);

        // Assert
        task.Title.Should().Be(title);
        task.IsDone.Should().BeFalse();
        task.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Constructor_CalledTwice_GeneratesDifferentIds()
    {
        var first = new TaskItem("اول");
        var second = new TaskItem("دوم");

        first.Id.Should().NotBe(second.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyTitle_ThrowsArgumentException(string? title)
    {
        var act = () => new TaskItem(title!);

        act.Should().Throw<ArgumentException>()
            .WithMessage("عنوان خالی است");
    }

    [Fact]
    public void Complete_WhenNotDone_MarksTaskAsDone()
    {
        var task = new TaskItem("خرید نان");

        task.Complete();

        task.IsDone.Should().BeTrue();
    }

    [Fact]
    public void Complete_WhenAlreadyDone_ThrowsInvalidOperationException()
    {
        var task = new TaskItem("خرید نان");
        task.Complete();

        var act = () => task.Complete();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("قبلاً انجام شده");
    }
}