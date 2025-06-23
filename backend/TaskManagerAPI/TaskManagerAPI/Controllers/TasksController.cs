using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManagerAPI.Data;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models.Common;
using TaskManagerAPI.Models.DTOs;
using TaskManagerAPI.Models.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagerAPI.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _task;
        private readonly ApplicationDBContext _context;
        public TasksController(ApplicationDBContext context, ITaskRepository task)
        {
            _task = task;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllTasks([FromQuery] QueryObject query)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var tasks = await _task.GetAllTasksAsync(query);
            if (tasks is null || !tasks.Any())
            {
                return NotFound("No tasks found.");
            }
            return Ok(tasks);
        }
        [HttpGet]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _task.GetTaskByIdAsync(id);
            if (task is null)
            {
                return NotFound("Task not found.");
            }
            return Ok(task);
        }

        [Authorize]
        [HttpPost]
        [Route("add/{userId}")]
        public async Task<IActionResult> CreateTask([FromRoute] string userId, [FromBody] CreateTaskDto taskItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userExists is null)
            {
                return BadRequest("Użytkownik nie istnieje 123123");
            }
            if (taskItem is null)
            {
                return BadRequest("Zadanie nie może być puste.");
            }
            var task = await _task.CreateTaskAsync(userId, taskItem);
            return Ok(task);
        }

        [Authorize]
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateTask([FromRoute] int id, [FromBody] UpdateTaskDto taskItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (taskItem is null)
            {
                return BadRequest("Zadanie nie może być puste.");
            }
            var task = await _task.UpdateTaskAsync(id, taskItem);
            if (task is null)
            {
                return NotFound("Nie znaleziono zadania o podanym ID.");
            }
            return Ok(task);
        }

        [Authorize]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskDeleted = await _task.DeleteTaskAsync(id);
            if (!taskDeleted)
            {
                return NotFound("Nie znaleziono zadania o podanym ID.");
            }
            return NoContent();
        }

        [Authorize]
        [HttpPatch]
        [Route("change-status/{id:int}")]
        public async Task<IActionResult> ChangeTaskStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskStatusChanged = await _task.ChangeTaskStatusAsync(id);
            if (!taskStatusChanged)
            {
                return NotFound("Nie znaleziono zadania o podanym ID.");
            }
            return Ok("Status zadania został zmieniony.");
        }
    }
}
