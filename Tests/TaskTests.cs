using Xunit;
using System.Collections.Generic;

namespace ToDoDotNet.Tests
{
    // Dummy Task class for demonstration. Replace with your actual model/logic.
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TaskService
    {
        private readonly List<Task> _tasks = new();
        private int _nextId = 1;

        public Task AddTask(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new System.ArgumentException("Title cannot be empty.");

            var task = new Task { Id = _nextId++, Title = title, IsCompleted = false };
            _tasks.Add(task);
            return task;
        }

        public bool UpdateTask(int id, string title)
        {
            var task = _tasks.Find(t => t.Id == id);
            if (task == null || string.IsNullOrWhiteSpace(title))
                return false;

            task.Title = title;
            return true;
        }

        public bool DeleteTask(int id)
        {
            var task = _tasks.Find(t => t.Id == id);
            if (task == null) return false;
            _tasks.Remove(task);
            return true;
        }

        public List<Task> GetTasks(bool? isCompleted = null)
        {
            if (isCompleted == null) return new List<Task>(_tasks);
            return _tasks.FindAll(t => t.IsCompleted == isCompleted.Value);
        }

        public bool MarkCompleted(int id)
        {
            var task = _tasks.Find(t => t.Id == id);
            if (task == null) return false;
            task.IsCompleted = true;
            return true;
        }
    }

    public class TaskTests
    {
        [Fact]
        public void CanAddTask()
        {
            var service = new TaskService();
            var task = service.AddTask("Test Task");
            Assert.NotNull(task);
            Assert.Equal("Test Task", task.Title);
            Assert.False(task.IsCompleted);
        }

        [Fact]
        public void CannotAddTaskWithEmptyTitle()
        {
            var service = new TaskService();
            Assert.Throws<System.ArgumentException>(() => service.AddTask(""));
        }

        [Fact]
        public void CanUpdateTaskTitle()
        {
            var service = new TaskService();
            var task = service.AddTask("Original Title");
            var updated = service.UpdateTask(task.Id, "Updated Title");
            Assert.True(updated);
            Assert.Equal("Updated Title", service.GetTasks()[0].Title);
        }

        [Fact]
        public void CannotUpdateNonexistentTask()
        {
            var service = new TaskService();
            var updated = service.UpdateTask(999, "Should Fail");
            Assert.False(updated);
        }

        [Fact]
        public void CanDeleteTask()
        {
            var service = new TaskService();
            var task = service.AddTask("Task to delete");
            var deleted = service.DeleteTask(task.Id);
            Assert.True(deleted);
            Assert.Empty(service.GetTasks());
        }

        [Fact]
        public void CanRetrieveTasks()
        {
            var service = new TaskService();
            service.AddTask("Task 1");
            service.AddTask("Task 2");
            var tasks = service.GetTasks();
            Assert.Equal(2, tasks.Count);
        }

        [Fact]
        public void CanMarkTaskCompleted()
        {
            var service = new TaskService();
            var task = service.AddTask("Task");
            var marked = service.MarkCompleted(task.Id);
            Assert.True(marked);
            Assert.True(service.GetTasks()[0].IsCompleted);
        }

        [Fact]
        public void CanFilterCompletedTasks()
        {
            var service = new TaskService();
            var t1 = service.AddTask("t1");
            var t2 = service.AddTask("t2");
            service.MarkCompleted(t2.Id);
            var completed = service.GetTasks(isCompleted: true);
            var pending = service.GetTasks(isCompleted: false);

            Assert.Single(completed);
            Assert.Single(pending);
            Assert.Equal(t2.Id, completed[0].Id);
            Assert.Equal(t1.Id, pending[0].Id);
        }
    }
}