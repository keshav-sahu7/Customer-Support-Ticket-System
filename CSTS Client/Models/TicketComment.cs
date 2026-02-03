using System;

namespace CSTS.Client.Models
{
    public class TicketComment
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid TicketId { get; set; }
        public Guid CreatedById { get; set; }
        public User? CreatedBy { get; set; }
    }
}
