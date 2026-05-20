using Xunit;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Services;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Tests
{
    public class TaskServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTaskAsync_WithValidData_ReturnsTask()
        {
            using var context = GetInMemoryDbContext();
            var user = new User
            {
                Email = "user@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                FirstName = "First",
                LastName = "Last"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var service = new TaskService(context);
            var dto = new CreateTaskDto
            {
                Title = "Test Task",
                Description = "Test Description",
                Priority = 3
            };

            var result = await service.CreateTaskAsync(user.Id, dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Title, result.Title);
            Assert.Equal(user.Id, result.UserId);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskByIdAsync_WithInvalidId_ThrowsException()
        {
            using var context = GetInMemoryDbContext();
            var service = new TaskService(context);

            await Assert.ThrowsAsync<Exception>(() => service.GetTaskByIdAsync(999));
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskAsync_WithValidId_ReturnsTrue()
        {
            using var context = GetInMemoryDbContext();
            var user = new User
            {
                Email = "user2@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                FirstName = "First",
                LastName = "Last"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var task = new TodoApi.Models.Task
            {
                UserId = user.Id,
                Title = "Test",
                Description = "Test",
                Priority = 2,
                DueDate = DateTime.UtcNow.AddDays(7)
            };
            context.Tasks.Add(task);
            await context.SaveChangesAsync();

            var service = new TaskService(context);

            var result = await service.DeleteTaskAsync(task.Id);

            Assert.True(result);
        }
    }
}
