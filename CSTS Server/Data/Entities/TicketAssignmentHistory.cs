using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSTS.Api.Data.Entities
{
    public class TicketAssignmentHistory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket? Ticket { get; set; }

        public Guid? OldAssignedToId { get; set; }

        [ForeignKey("OldAssignedToId")]
        public User? OldAssignedTo { get; set; }

        public Guid? NewAssignedToId { get; set; }

        [ForeignKey("NewAssignedToId")]
        public User? NewAssignedTo { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; }

        [Required]
        public Guid ChangedById { get; set; }

        [ForeignKey("ChangedById")]
        public User? ChangedBy { get; set; }
    }
}
