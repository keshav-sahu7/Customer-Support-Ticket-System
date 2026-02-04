using System;
using System.ComponentModel.DataAnnotations;

namespace CSTS.Api.Dtos
{
    public class AssignTicketRequest
    {
        [Required]
        public Guid AssignToId { get; set; }
    }
}
