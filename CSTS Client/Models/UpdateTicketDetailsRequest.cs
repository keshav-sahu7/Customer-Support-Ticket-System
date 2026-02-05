namespace CSTS.Client.Models
{
    public class UpdateTicketDetailsRequest
    {
        public Guid UserId { get; set; }
        public Guid TicketId { get; set; }
        public Guid? AssigneeId { get; set; }
        public string? Status { get; set; }
    }
}
