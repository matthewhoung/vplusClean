using Core.Tasks;

namespace Application.Interfaces.Tasks
{
    public interface ITaskRepository
    {
        //TODO:
        //create index inside the database for the status column
        //create index inside the database for the priority column
        //create index inside the database for the isCompleted column
        //Fix all the get methods
        //index all the get methods
        //Progress service methods
        //isArchived service methods
        //index service methods
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
        Task<int> AddWorkDayAsync(WorkDay workDay);
        /*
         Update Section
         - UpdateWorkDayCompletionAsync
         */
        Task UpdateWorkDayCompletionAsync(int workDayId);
    }
}
