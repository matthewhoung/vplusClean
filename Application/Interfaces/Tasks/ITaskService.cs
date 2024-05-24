using Core.Tasks;

namespace Application.Interfaces.Tasks
{
    public interface ITaskService
    {
        Task<TaskBody> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskSubBody>> GetSubTaskAsync(int taskId);
        Task<IEnumerable<Collaborator>> GetCollaboratorsAsync(int taskId);
        Task<IEnumerable<WorkDay>> GetnotCompletedWorkDaysAsync(int taskId);
        Task AddTaskAsync(TaskBody task);
        Task AddSubTaskAsync(TaskSubBody subTask);
        Task AddCollaboratorAsync(Collaborator collaborator);
        Task<int> AddWorkDayAsync(WorkDay workDay);
        Task UpdateWorkDayCompletionAsync(int workDayId);
    }
}
