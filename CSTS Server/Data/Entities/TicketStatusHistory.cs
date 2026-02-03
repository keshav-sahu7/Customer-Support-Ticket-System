using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSTS.Api.Data.Entities
{
    public class TicketStatusHistory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket? Ticket { get; set; }

        [Required]
        public TicketStatus OldStatus { get; set; }

        [Required]
        public TicketStatus NewStatus { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; }

        [Required]
        public Guid ChangedById { get; set; }

        [ForeignKey("ChangedById")]
        public User? ChangedBy { get; set; }
    }
}
