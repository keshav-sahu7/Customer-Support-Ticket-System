using System;

namespace CSTS.Client.Models
{
    public class TicketComment
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedByUsername { get; set; }

        // Computed property to return CreatedAt in local time
        public DateTime CreatedAtLocal => CreatedAt.ToLocalTime();
    }
}
