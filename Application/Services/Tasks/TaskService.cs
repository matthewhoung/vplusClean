using Application.Interfaces.Tasks;
using AutoMapper;
using Core.Tasks;
using WebAPI.DTOs.Tasks;

namespace Application.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<TaskBody> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id);
        }

        public async Task AddTaskAsync(TaskDto task)
        {
            var taskDto = _mapper.Map<TaskBody>(task);
            await _taskRepository.AddTaskAsync(taskDto);
        }

        public async Task AddSubTaskAsync(TaskSubBody subTask)
        {
            await _taskRepository.AddSubTaskAsync(subTask);
        }

        public async Task AddCollaboratorAsync(Collaborator collaborator)
        {
            await _taskRepository.AddCollaboratorAsync(collaborator);
        }

        public async Task AddWorkDayAsync(WorkDay workDay)
        {
            await _taskRepository.AddWorkDayAsync(workDay);
        }

        public async Task UpdateWorkDayCompletionAsync(int workDayId)
        {
            await _taskRepository.UpdateWorkDayCompletionAsync(workDayId);
        }
    }
}
