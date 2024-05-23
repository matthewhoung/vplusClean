using Core.Tasks;

namespace Application.Interfaces.Tasks
{
    public interface ITaskRepository
    {
        /*
        Retrieve Section
        - GetTaskByIdAsync
        - GetSubTaskAsync
        - GetCollaboratorAsync
        - GetWorkDayAsync
        */
        Task<TaskBody> GetTaskByIdAsync(int userId);
        Task<IEnumerable<TaskSubBody>> GetSubTaskAsync(int taskId);
        Task<IEnumerable<Collaborator>> GetCollaboratorAsync(int taskId);
        Task<IEnumerable<WorkDay>> GetWorkDayAsync(int taskId);
        /*
         Create Section
         - AddTaskAsync
         - AddSubTaskAsync 
         - AddCollaboratorAsync
         - AddWorkDayAsync
         */
        Task AddTaskAsync(TaskBody task);
        Task AddSubTaskAsync(TaskSubBody subTask);
        Task AddCollaboratorAsync(Collaborator collaborator);
        Task AddWorkDayAsync(WorkDay workDay);
        /*
         Update Section
         - UpdateWorkDayCompletionAsync
         */
        Task UpdateWorkDayCompletionAsync(int workDayId);
    }
}
