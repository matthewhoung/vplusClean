namespace Application.Interfaces.Tasks
{
    public interface ITaskGeneric<T>
        where T : class
    {
        Task<T> GetTaskByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id, int taskId);
    }
}
