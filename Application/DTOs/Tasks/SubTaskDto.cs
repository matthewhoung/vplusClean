﻿namespace Application.DTOs.Tasks
{
    public class SubTaskDto
    {
        public int? Id { get; set; }
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public decimal Progress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
