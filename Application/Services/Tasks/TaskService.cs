using Application.Interfaces.Tasks;
using AutoMapper;
using Core.Tasks;

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
        public async Task<IEnumerable<TaskSubBody>> GetSubTaskAsync(int taskId)
        {
            return await _taskRepository.GetSubTaskAsync(taskId);
        }
        public async Task<IEnumerable<Collaborator>> GetCollaboratorsAsync(int taskId)
        {
            return await _taskRepository.GetCollaboratorAsync(taskId);
        }
        public async Task<IEnumerable<WorkDay>> GetnotCompletedWorkDaysAsync(int taskId)
        {
            return await _taskRepository.GetWorkDayAsync(taskId);
        }

        public async Task AddTaskAsync(TaskBody task)
        {
            await _taskRepository.AddTaskAsync(task);
        }

        public async Task AddSubTaskAsync(TaskSubBody subTask)
        {
            await _taskRepository.AddSubTaskAsync(subTask);
        }

        public async Task AddCollaboratorAsync(Collaborator collaborator)
        {
            await _taskRepository.AddCollaboratorAsync(collaborator);
        }

        public async Task<int> AddWorkDayAsync(WorkDay workDay)
        {
            await _taskRepository.AddWorkDayAsync(workDay);
            return workDay.WorkDayId;
        }

        public async Task UpdateWorkDayCompletionAsync(int workDayId)
        {
            await _taskRepository.UpdateWorkDayCompletionAsync(workDayId);
        }
    }
}
