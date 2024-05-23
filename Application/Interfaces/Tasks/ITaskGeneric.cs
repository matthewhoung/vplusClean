namespace Application.Interfaces.Tasks
{
    public interface ITaskGeneric<T>
        where T : class
    {
        Task<T> GetTaskByIdAsync(int userId);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int taskId);
    }
}
