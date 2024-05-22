namespace Core.Tasks
{
    public class TaskSubBody
    {
        public int SubTaskId { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public decimal Progress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Collaborator> Collaborators { get; set; } = new List<Collaborator>();
        public List<WorkDay> WorkDays { get; set; } = new List<WorkDay>();
    }
}
