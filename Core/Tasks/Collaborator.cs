﻿namespace Core.Tasks
{
    public class Collaborator
    {
        public int CollabId { get; set; }
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public int? SubTaskId { get; set; }
    }
}
