using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;
using Task = TodoApi.Models.Task;

namespace TodoApi.Services
{
    /// <summary>
    /// Serviço de tarefas - Lógica de CRUD de tarefas
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter uma tarefa pelo ID
        /// </summary>
        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new Exception("Tarefa não encontrada");

            return MapToDto(task);
        }

        /// <summary>
        /// Obter todas as tarefas de um usuário
        /// </summary>
        public async Task<IEnumerable<TaskDto>> GetAllTasksByUserAsync(int userId)
        {
            // Validar que o usuário existe
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                throw new Exception("Usuário não encontrado");

            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return tasks.Select(MapToDto);
        }

        /// <summary>
        /// Criar nova tarefa
        /// </summary>
        public async Task<TaskDto> CreateTaskAsync(int userId, CreateTaskDto dto)
        {
            // Validar que o usuário existe
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                throw new Exception("Usuário não encontrado");

            var task = new Task
            {
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                DueDate = dto.DueDate ?? DateTime.UtcNow.AddDays(7),
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        /// <summary>
        /// Atualizar uma tarefa
        /// </summary>
        public async Task<TaskDto> UpdateTaskAsync(int id, CreateTaskDto dto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new Exception("Tarefa não encontrada");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate ?? task.DueDate;
            task.UpdatedAt = DateTime.UtcNow;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        /// <summary>
        /// Deletar uma tarefa
        /// </summary>
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new Exception("Tarefa não encontrada");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Marcar tarefa como concluída
        /// </summary>
        public async Task<bool> CompleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new Exception("Tarefa não encontrada");

            task.IsCompleted = true;
            task.UpdatedAt = DateTime.UtcNow;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return true;
        }

        // ===== MÉTODO PRIVADO =====

        /// <summary>
        /// Mapear Task para TaskDto
        /// </summary>
        private TaskDto MapToDto(TodoApi.Models.Task task)
        {
            return new TaskDto
            {
                Id = task.Id,
                UserId = task.UserId,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                Priority = task.Priority,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }
    }
}