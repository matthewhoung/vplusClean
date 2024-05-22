using Core.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost("add/{id}/task")]
        public async Task<IActionResult> AddTask([FromBody] TaskBody task)
        {
            await _taskService.AddTaskAsync(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpPost("add/{id}/subtasks")]
        public async Task<IActionResult> AddSubTask(int id, [FromBody] TaskSubBody subTask)
        {
            subTask.TaskId = id;
            await _taskService.AddSubTaskAsync(subTask);
            return CreatedAtAction(nameof(GetTaskById), new { id = subTask.SubTaskId }, subTask);
        }
        [HttpPost("add/{id}/collaborators")]
        public async Task<IActionResult> AddCollaborator(int id, [FromBody] Collaborator collaborator)
        {
            collaborator.TaskId = id;
            await _taskService.AddCollaboratorAsync(collaborator);
            return CreatedAtAction(nameof(GetTaskById), new { id = collaborator.CollabId }, collaborator);
        }

        [HttpPost("add/{id}/workdays")]
        public async Task<IActionResult> AddWorkDay(int id, [FromBody] WorkDay workDay)
        {
            workDay.TaskId = id;
            await _taskService.AddWorkDayAsync(workDay);
            return CreatedAtAction(nameof(GetTaskById), new { id = workDay.WorkDayId }, workDay);
        }

        [HttpPut("update/{id}/{task}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskBody task)
        {
            if (id != task.Id) return BadRequest();
            await _taskService.UpdateTaskAsync(task);
            return NoContent();
        }

        [HttpDelete("delete/{id}/{task}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}
