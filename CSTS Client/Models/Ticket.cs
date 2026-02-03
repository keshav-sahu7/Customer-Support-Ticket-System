using System;

namespace CSTS.Client.Models
{
    public enum TicketPriority
    {
        Low,
        Medium,
        High
    }

    public enum TicketStatus
    {
        Open,
        InProgress,
        Closed
    }

    public class Ticket
    {
        public Guid Id { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedById { get; set; }
        public User? CreatedBy { get; set; }
        public Guid? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }
    }
}
