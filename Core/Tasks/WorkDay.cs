namespace Core.Tasks
{
    public class WorkDay
    {
        public int WorkDayId { get; set; }
        public int TaskId { get; set; }
        public int? SubTaskId { get; set; }
        public DateTime WorkDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
