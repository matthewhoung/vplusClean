using Core.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Tasks;
using Microsoft.AspNetCore.Authorization;
using Application.DTOs.Tasks;
using AutoMapper;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TasksController(ITaskService taskService, IMapper mapper, ILogger logger)
        {
            _taskService = taskService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost("add/task")]
        public async Task<IActionResult> AddTask([FromBody] TaskDto task)
        {
            try
            {
                var addTask = _mapper.Map<TaskBody>(task);
                await _taskService.AddTaskAsync(addTask);

                return Ok(addTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("add/{id}/subtask")]
        public async Task<IActionResult> AddSubTask(int id, [FromBody] TaskSubBody subTask)
        {
            if (subTask == null) return BadRequest();

            subTask.TaskId = id;
            await _taskService.AddSubTaskAsync(subTask);
            return CreatedAtAction(nameof(GetTaskById), new { id = subTask.SubTaskId }, subTask);
        }

        [HttpPost("add/{id}/collaborator")]
        public async Task<IActionResult> AddCollaborator(int id, [FromBody] Collaborator collaborator)
        {
            if (collaborator == null) return BadRequest();

            collaborator.TaskId = id;
            await _taskService.AddCollaboratorAsync(collaborator);
            return CreatedAtAction(nameof(GetTaskById), new { id = collaborator.CollabId }, collaborator);
        }

        [HttpPost("add/{id}/workday")]
        public async Task<IActionResult> AddWorkDay(int id, [FromBody] WorkDay workDay)
        {
            if (workDay == null) return BadRequest();

            workDay.TaskId = id;
            await _taskService.AddWorkDayAsync(workDay);
            return CreatedAtAction(nameof(GetTaskById), new { id = workDay.WorkDayId }, workDay);
        }
    }
}
