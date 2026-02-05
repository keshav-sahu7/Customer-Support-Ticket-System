using System.ComponentModel.DataAnnotations;

namespace CSTS.Api.Dtos
{
    public class CreateTicketRequest
    {
        [Required]
        [MaxLength(100)]
        public string? Subject { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Priority { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}
