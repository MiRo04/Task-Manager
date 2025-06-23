using TaskManagerAPI.Models.Common;
using TaskManagerAPI.Models.DTOs;
using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskItems>> GetAllTasksAsync(QueryObject query);
        Task<TaskItems?> GetTaskByIdAsync(int id);
        Task<TaskItemDto?> CreateTaskAsync(string userId, CreateTaskDto taskItem);
        Task<TaskItemDto?> UpdateTaskAsync(int id, UpdateTaskDto taskItem);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> ChangeTaskStatusAsync(int id);
    }
}
