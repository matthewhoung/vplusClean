using Application.DTOs.Tasks;
using Application.Interfaces.Tasks;
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
        /*
        Retrieve Section
        - GetTaskByIdAsync
        - GetSubTaskAsync
        - GetCollaboratorAsync
        - GetWorkDayAsync
        */
        public async Task<TaskBody> GetTaskByIdAsync(int userId)
        {
            var query = @"
                SELECT t.* 
                FROM tasks t
                WHERE t.user_id = @UserId";
            var taskId = await _dbConnection.QueryFirstOrDefaultAsync<TaskBody>(query, new {UserId = userId });
            if (taskId != null)
            {
                taskId.SubTasks = (await GetSubTaskAsync(userId)).ToList();
                taskId.Collaborators = (await GetCollaboratorAsync(userId)).ToList();
                taskId.WorkDays = (await GetWorkDayAsync(userId)).ToList();
            }
            return taskId;
        }
        public async Task<IEnumerable<TaskSubBody>> GetSubTaskAsync(int taskId)
        {
            var query = @"
                SELECT 
                    t.id, t.user_id, t.title, t.description, t.priority, t.statuts, t.progress, t.start_date, t.end_date,
                    s.sub_id, s.task_id, s.title, s.description, s.priority, s.statuts, s.progress, s.start_date, s.end_date
                FROM tasks t
                LEFT JOIN tasks_sub s ON t.id = s.task_id
                WHERE t.Id = @TaskId";

            var taskDict = new Dictionary<int, TaskBody>();

            var result = await _dbConnection.QueryAsync<TaskBody, TaskSubBody, TaskBody>(
                query,
                (task, subTask) =>
                {
                    if (!taskDict.TryGetValue(task.Id, out var currentTask))
                    {
                        currentTask = task;
                        currentTask.SubTasks = new List<TaskSubBody>();
                        taskDict.Add(currentTask.Id, currentTask);
                    }

                    if (subTask != null)
                    {
                        currentTask.SubTasks.Add(subTask);
                    }

                    return currentTask;
                },
                new { TaskId = taskId },
                splitOn: "sub_id"
            );

            return taskDict.Values.SelectMany(t => t.SubTasks);
        }
        public async Task<IEnumerable<Collaborator>> GetCollaboratorAsync(int taskId)
        {
            var querry = @"
                SELECT
                    c.* 
                FROM tasks_collaborator c
                WHERE c.task_id = @TaskId";
            var collaborators = await _dbConnection.QueryAsync<Collaborator>(querry, new { TaskId = taskId });
            return collaborators;
        }
        public async Task<IEnumerable<WorkDay>> GetWorkDayAsync(int taskId)
        {
            var query = @"
                SELECT 
                    w.workday_id, w.task_id, w.sub_task_id, w.work_date, w.is_completed,
                    t.id, t.user_id, t.title, t.description, t.priority, t.statuts, t.progress, t.start_date, t.end_date,
                    s.sub_id, s.task_id, s.title, s.description, s.priority, s.statuts, s.progress, s.start_date, s.end_date
                FROM tasks_workday w
                LEFT JOIN tasks t ON w.task_id = t.id
                LEFT JOIN tasks_sub s ON w.sub_task_id = s.sub_id
                WHERE w.task_id = @TaskId AND w.is_completed = 0";

            var taskDict = new Dictionary<int, TaskBody>();

            var result = await _dbConnection.QueryAsync<TaskBody, TaskSubBody, WorkDay, TaskBody>(
                query,
                (task, subTask, workDay) =>
                {
                    if (!taskDict.TryGetValue(task.Id, out var currentTask))
                    {
                        currentTask = task;
                        currentTask.SubTasks = new List<TaskSubBody>();
                        currentTask.WorkDays = new List<WorkDay>();
                        taskDict.Add(currentTask.Id, currentTask);
                    }

                    if (subTask != null)
                    {
                        var subTaskItem = currentTask.SubTasks.FirstOrDefault(st => st.SubTaskId == subTask.SubTaskId);
                        if (subTaskItem == null)
                        {
                            subTask.WorkDays = new List<WorkDay>();
                            currentTask.SubTasks.Add(subTask);
                        }
                        subTaskItem?.WorkDays.Add(workDay);
                    }

                    currentTask.WorkDays.Add(workDay);

                    return currentTask;
                },
                new { TaskId = taskId },
                splitOn: "sub_id,workday_id"
            );

            return taskDict.Values.SelectMany(t => t.WorkDays).Where(w => w.IsCompleted == false);
        }
        /*
         Create Section
         - AddTaskAsync
         - AddSubTaskAsync 
         - AddCollaboratorAsync
         - AddWorkDayAsync
         */
        public async Task AddTaskAsync(TaskBody task)
        {
            _dbConnection.Open();
            using var transaction = _dbConnection.BeginTransaction();
            try
            {
                var command = @"
                    INSERT INTO tasks (user_id, title, description, priority, status, progress, start_date, end_date)
                    VALUES(@UserId, @Title, @Description, @Priority, @Status, @Progress, @StartDate, @EndDate)";
                await _dbConnection.ExecuteAsync(command, task, transaction);
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
            _dbConnection.Open();
            using var transaction = _dbConnection.BeginTransaction();
            try
            {
                var command = @"
                    INSERT INTO tasks_sub (task_id, title, description, priority, status, progress, start_date, end_date)
                    VALUES(@TaskId, @Title, @Description, @Priority, @Status, @Progress, @StartDate, @EndDate)";
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
            _dbConnection.Open();
            using var transaction = _dbConnection.BeginTransaction();
            try
            {
                var command = @"
                    INSERT INTO tasks_collaborator (user_id, task_id, sub_task_id)
                    VALUES(@UserId, @TaskId, @SubTaskId)";
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
        public async Task<int> AddWorkDayAsync(WorkDay workDay)
        {
            _dbConnection.Open();
            using var transaction = _dbConnection.BeginTransaction();
            try
            {
                var command = @"
                    INSERT INTO tasks_workday (task_id, sub_task_id, work_date, is_completed)
                    VALUES(@TaskId, @SubTaskId, @WorkDate, @IsCompleted);
                    SELECT LAST_INSERT_ID();";

                var workDayId = await _dbConnection.ExecuteScalarAsync<int>(command, workDay, transaction);
                workDay.WorkDayId = workDayId;
                transaction.Commit();
                return workDayId;
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
        /*
         Update Section
         - UpdateWorkDayCompletionAsync
         */
        public async Task UpdateWorkDayCompletionAsync(int workDayId)
        {
            _dbConnection.Open();
            using var transaction = _dbConnection.BeginTransaction();
            try
            {
                var command = @"
                    UPDATE tasks_workday
                    SET is_completed = 1
                    WHERE workday_id = @WorkDayId";

                await _dbConnection.ExecuteAsync(command, new { WorkDayId = workDayId });
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
    }
}
