namespace CSTS.Client.Models
{
    public class CreateTicketRequest
    {
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public Guid? UserId { get; set; }
    }
}
