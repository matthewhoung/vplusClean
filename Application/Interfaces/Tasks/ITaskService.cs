using Core.Tasks;

namespace Application.Interfaces.Tasks
{
    public interface ITaskService
    {
        Task<TaskBody> GetTaskByIdAsync(int id);
        Task AddTaskAsync(TaskBody task);
        Task UpdateTaskAsync(TaskBody task);
        Task DeleteTaskAsync(int id);
        Task AddSubTaskAsync(TaskSubBody subTask);
        Task AddCollaboratorAsync(Collaborator collaborator);
        Task AddWorkDayAsync(WorkDay workDay);
    }
}
