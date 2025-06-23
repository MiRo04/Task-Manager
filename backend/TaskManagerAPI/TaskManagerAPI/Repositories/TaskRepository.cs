using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models.Common;
using TaskManagerAPI.Models.DTOs;
using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDBContext _context;
        public TaskRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> ChangeTaskStatusAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task is null)
            {
                return false;
            }
            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TaskItemDto?> CreateTaskAsync(string userId, CreateTaskDto taskItem)
        {
            var task = new TaskItems
            {
                Title = taskItem.Title,
                CreatedAt = DateTime.UtcNow,
                Description = taskItem.Description,
                DueDate = taskItem.DueDate,
                UserId = userId
            };
            await _context.TaskItems.AddAsync(task);
            await _context.SaveChangesAsync();
            var taskDto = new TaskItemDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                UserId = task.UserId
            };
            return taskDto;

        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task is null)
            {
                return false;
            }
            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TaskItems>> GetAllTasksAsync(QueryObject query)
        {
            var tasks = _context.TaskItems.AsNoTracking().AsQueryable();
            if(!string.IsNullOrEmpty(query.userId))
            {
                tasks = tasks.Where(t => t.UserId.Contains(query.userId));
            }
            if (tasks is null || !tasks.Any())
            {
                return new List<TaskItems>();
            }
            return await tasks.ToListAsync();
        }

        public async Task<TaskItems?> GetTaskByIdAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task is null)
            {
                return null;
            }
            return task;
        }

        public async Task<TaskItemDto?> UpdateTaskAsync(int id, UpdateTaskDto taskItem)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task is null)
            {
                return null;
            }
            task.Title = taskItem.Title ?? task.Title;
            task.Description = taskItem.Description ?? task.Description;
            task.DueDate = taskItem.DueDate ?? task.DueDate;
            task.IsCompleted = taskItem.IsCompleted;
            var taskDto = new TaskItemDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                UserId = task.UserId
            };
            await _context.SaveChangesAsync();
            return taskDto;
        }
    }
}
