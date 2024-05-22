using Core.Tasks;

namespace Application.Interfaces
{
    public interface ITaskRepository
    {
        Task AddTaskAsync(TaskBody task);
        Task AddSubTaskAsync(TaskSubBody subTask);
        Task AddCollaboratorAsync(Collaborator collaborator);
        Task AddWorkDayAsync(WorkDay workDay);
        Task<IEnumerable<TaskSubBody>> GetSubTaskAsync(int taskId);
        Task<IEnumerable<Collaborator>> GetCollaboratorAsync(int taskId);
        Task<IEnumerable<WorkDay>> GetWorkDayAsync(int taskId);
    }
}
