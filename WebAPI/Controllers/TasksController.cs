using Core.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Tasks;
using Application.DTOs.Tasks;
using AutoMapper;

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

        [HttpGet("subtasks/{taskId}")]
        public async Task<IActionResult> GetSubTask(int taskId)
        {
            var subTasks = await _taskService.GetSubTaskAsync(taskId);
            if (subTasks == null) return NotFound();
            return Ok(subTasks);
        }

        [HttpGet("collaborators/{taskId}")]
        public async Task<IActionResult> GetCollaborators(int taskId)
        {
            var collaborators = await _taskService.GetCollaboratorsAsync(taskId);
            if (collaborators == null) return NotFound();
            return Ok(collaborators);
        }

        [HttpGet("workdays/notcomplete/{taskId}")]
        public async Task<IActionResult> GetNotCompletedWorkDays(int taskId)
        {
            var workDays = await _taskService.GetnotCompletedWorkDaysAsync(taskId);
            if (workDays == null) return NotFound();
            return Ok(workDays);
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
        public async Task<int> AddWorkDayAsync(WorkDay workDay)
        {
            try
            {
                _logger.LogInformation("Adding workday: {@WorkDay}", workDay);

                var workDayId = await _taskService.AddWorkDayAsync(workDay);

                _logger.LogInformation("Successfully added workday with WorkDayId: {WorkDayId}", workDayId);
                return workDayId;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding workday");
                return -1;
            }
            
        }

        [HttpPut("update/workday/{workDayId}")]
        public async Task<IActionResult> UpdateWorkDayCompletion(int workDayId)
        {
            try
            {
                
                _logger.LogInformation("Updating workday completion for WorkDayId: {WorkDayId}", workDayId);

                await _taskService.UpdateWorkDayCompletionAsync(workDayId);

                _logger.LogInformation("Successfully updated workday completion for WorkDayId: {WorkDayId}", workDayId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workday completion for WorkDayId: {WorkDayId}", workDayId);
                return StatusCode(500, new { message = "An error occurred while updating the workday completion" });
            }
        }
    }
}
