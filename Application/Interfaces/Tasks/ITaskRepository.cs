using Core.Tasks;

namespace Application.Interfaces.Tasks
{
    public interface ITaskRepository : ITaskGeneric<TaskBody>
    {
        //ToDo: add progress, priority, start date, end date
        Task AddSubTaskAsync(TaskSubBody subTask);
        Task AddCollaboratorAsync(Collaborator collaborator);
        Task AddWorkDayAsync(WorkDay workDay);
        Task<IEnumerable<TaskSubBody>> GetSubTaskAsync(int taskId);
        Task<IEnumerable<Collaborator>> GetCollaboratorAsync(int taskId);
        Task<IEnumerable<WorkDay>> GetWorkDayAsync(int taskId);
    }
}
