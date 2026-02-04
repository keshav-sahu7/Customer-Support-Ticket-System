using System.ComponentModel.DataAnnotations;

namespace CSTS.Api.Dtos
{
    public class UpdateTicketStatusRequest
    {
        [Required]
        public string? Status { get; set; }
    }
}
