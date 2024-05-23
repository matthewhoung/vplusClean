using Core.Tasks;

namespace Application.Interfaces.Tasks
{
    public interface ITaskService
    {
        Task<TaskBody> GetTaskByIdAsync(int id);
        Task AddTaskAsync(TaskBody task);
        Task AddSubTaskAsync(TaskSubBody subTask);
        Task AddCollaboratorAsync(Collaborator collaborator);
        Task AddWorkDayAsync(WorkDay workDay);
        Task UpdateWorkDayCompletionAsync(int workDayId);
    }
}
