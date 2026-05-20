using TodoApi.DTOs;

namespace TodoApi.Services
{
    /// <summary>
    /// Interface do serviço de tarefas
    /// </summary>
    public interface ITaskService
    {
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskDto>> GetAllTasksByUserAsync(int userId);
        Task<TaskDto> CreateTaskAsync(int userId, CreateTaskDto dto);
        Task<TaskDto> UpdateTaskAsync(int id, CreateTaskDto dto);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> CompleteTaskAsync(int Id);
    }
}
