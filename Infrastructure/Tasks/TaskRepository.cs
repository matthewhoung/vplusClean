using Application.Interfaces;
using Core.Tasks;
using Dapper;
using System.Data;

namespace Infrastructure.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbConnection _dbConnection;

        public TaskRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        
        public async Task<TaskBody> GetAllByIdAsync(int id)
        {
            // TODO: modify the table and column names to match the actual database schema
            var query = "SELECT * FROM Tasks WHERE Id = @Id";
            var taskId = await _dbConnection.QueryFirstOrDefaultAsync<TaskBody>(query, new {Id = id});
            if (taskId != null)
            {
                taskId.SubTasks = (await GetSubTaskAsync(id)).ToList();
                taskId.Collaborators = (await GetCollaboratorAsync(id)).ToList();
                taskId.WorkDays = (await GetWorkDayAsync(id)).ToList();
            }
            return taskId;
        }
        public async Task AddTaskAsync(TaskBody task)
        {
            // TODO: modify the table and column names to match the actual database schema
            _dbConnection.Open();
            using var transaction = _dbConnection.BeginTransaction();
            try
            {
                var command = @"
                    INSERT INTO Tasks (UserId, Title, Description, Priority, Progress, StartDate, EndDate)
                    VALUES(@UserId, @Title, @Description, @Priority, @Progress, @StartDate, @EndDate)";
                await _dbConnection.ExecuteAsync(command, task);
                transaction.Commit();
            }
            catch(Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                _dbConnection.Close();
            }
        }
        public async Task AddSubTaskAsync(TaskSubBody subTask)
        {
            // TODO: modify the table and column names to match the actual database schema
            _dbConnection.Open();
            using var transaction = _dbConnection.BeginTransaction();
            try
            {
                var command = @"
                    INSERT INTO SubTasks (TaskId, UserId, Title, Description, Priority, Progress, StartDate, EndDate)
                    VALUES(@TaskId, @UserId, @Title, @Description, @Priority, @Progress, @StartDate, @EndDate)";
                await _dbConnection.ExecuteAsync(command, subTask);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                _dbConnection.Close();
            }
        }
        public async Task AddCollaboratorAsync(Collaborator collaborator)
        {
            // TODO: modify the table and column names to match the actual database schema
            _dbConnection.Open();
            using var transaction = _dbConnection.BeginTransaction();
            try
            {
                var command = @"
                    INSERT INTO Collaborators (TaskId, UserId)
                    VALUES(@TaskId, @UserId)";
                await _dbConnection.ExecuteAsync(command, collaborator);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                _dbConnection.Close();
            }
        }
        public async Task AddWorkDayAsync(WorkDay workDay)
        {
            _dbConnection.Open();
            using var transaction = _dbConnection.BeginTransaction();
            try
            {
                var command = @"
                    INSERT INTO WorkDays (TaskId, UserId, Date, Hours)
                    VALUES(@TaskId, @UserId, @Date, @Hours)";
                await _dbConnection.ExecuteAsync(command, workDay);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                _dbConnection.Close();
            }
        }
        public async Task<IEnumerable<TaskSubBody>> GetSubTaskAsync(int taskId)
        {
            var querry = "SELECT * FROM SubTasks WHERE TaskId = @TaskId";
            var subTasks = await _dbConnection.QueryAsync<TaskSubBody>(querry, new { TaskId = taskId });
            return subTasks;
        }
        public async Task<IEnumerable<Collaborator>> GetCollaboratorAsync(int taskId)
        {
            var querry = "SELECT * FROM Collaborators WHERE TaskId = @TaskId";
            var collaborators = await _dbConnection.QueryAsync<Collaborator>(querry, new { TaskId = taskId });
            return collaborators;
        }
        public async Task<IEnumerable<WorkDay>> GetWorkDayAsync(int taskId)
        {
            var querry = "SELECT * FROM WorkDays WHERE TaskId = @TaskId";
            var workDays = await _dbConnection.QueryAsync<WorkDay>(querry, new { TaskId = taskId });
            return workDays;
        }
    }
}
