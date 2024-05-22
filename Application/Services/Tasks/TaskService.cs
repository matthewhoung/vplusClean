using Application.Interfaces.Tasks;
using Core.Tasks;

namespace Application.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<TaskBody> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id);
        }

        public async Task AddTaskAsync(TaskBody task)
        {
            await _taskRepository.AddAsync(task);
        }

        public async Task UpdateTaskAsync(TaskBody task)
        {
            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteAsync(id);
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
    }
}
