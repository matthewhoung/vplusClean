using Core.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Tasks;
using Microsoft.AspNetCore.Authorization;
using Application.DTOs.Tasks;
using AutoMapper;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, IMapper mapper, ILogger<TasksController> logger)
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
                _logger.LogInformation("Adding task: {@Task}", task);

                var addTask = _mapper.Map<TaskBody>(task);
                _logger.LogInformation("Mapped TaskBody: {@TaskBody}", addTask);

                await _taskService.AddTaskAsync(addTask);

                return Ok(addTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding task");
                return BadRequest(new { message = "An error occurred while adding the task" });
            }
        }

        [HttpPost("add/subtask")]
        public async Task<IActionResult> AddSubTask([FromBody] SubTaskDto subTask)
        {
            try
            {
                _logger.LogInformation("Adding sub task: {@SubTask}", subTask);

                var addSubTask = _mapper.Map<TaskSubBody>(subTask);
                _logger.LogInformation("Mapped SubTaskBody: {@TaskSubBody}", addSubTask);

                await _taskService.AddSubTaskAsync(addSubTask);

                return Ok(addSubTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding task");
                return BadRequest(new { message = "An error occurred while adding the task" });
            }
        }

        [HttpPost("add/collaborator")]
        public async Task<IActionResult> AddCollaborator([FromBody] Collaborator collaborator)
        {
            if (collaborator == null) return BadRequest();

            await _taskService.AddCollaboratorAsync(collaborator);
            return Ok(collaborator);
        }

        [HttpPost("add/workday")]
        public async Task<IActionResult> AddWorkDay([FromBody] WorkDay workDay)
        {
            if (workDay == null) return BadRequest();

            await _taskService.AddWorkDayAsync(workDay);
            return Ok(workDay);
        }
    }
}
