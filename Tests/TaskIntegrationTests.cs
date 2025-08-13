using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace ToDoDotNet.Tests
{
    public class TaskIntegrationTests
    {
        private TaskService _service;

        public TaskIntegrationTests()
        {
            _service = new TaskService();
        }

        [Fact]
        public void Should_AddMultipleTasks_AndRetrieveThem()
        {
            var titles = new[] { "Task 1", "Task 2", "Task 3" };
            foreach (var title in titles)
                _service.AddTask(title);

            var retrieved = _service.GetTasks();
            Assert.Equal(titles.Length, retrieved.Count);
            Assert.All(retrieved, t => Assert.Contains(t.Title, titles));
        }

        [Fact]
        public void Should_MarkTaskCompleted_OnlyAffectsSpecifiedTask()
        {
            var t1 = _service.AddTask("t1");
            var t2 = _service.AddTask("t2");

            _service.MarkCompleted(t2.Id);

            var tasks = _service.GetTasks();
            Assert.False(tasks.First(x => x.Id == t1.Id).IsCompleted);
            Assert.True(tasks.First(x => x.Id == t2.Id).IsCompleted);
        }

        [Fact]
        public void Should_DeleteTask_AndRemainOthersIntact()
        {
            var t1 = _service.AddTask("t1");
            var t2 = _service.AddTask("t2");

            var deleted = _service.DeleteTask(t1.Id);
            Assert.True(deleted);

            var remaining = _service.GetTasks();
            Assert.Single(remaining);
            Assert.Equal(t2.Id, remaining[0].Id);
        }

        [Fact]
        public void Should_NotUpdateOrDelete_NonexistentTask()
        {
            var updateResult = _service.UpdateTask(9999, "Nope");
            var deleteResult = _service.DeleteTask(8888);

            Assert.False(updateResult);
            Assert.False(deleteResult);
        }

        [Fact]
        public void Should_ThrowException_WhenAddingTaskWithNullOrWhitespaceTitle()
        {
            Assert.Throws<System.ArgumentException>(() => _service.AddTask(null));
            Assert.Throws<System.ArgumentException>(() => _service.AddTask(""));
            Assert.Throws<System.ArgumentException>(() => _service.AddTask(" ")); 
        }
    }
}