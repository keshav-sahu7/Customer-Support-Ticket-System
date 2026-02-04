using System.ComponentModel.DataAnnotations;

namespace CSTS.Api.Dtos
{
    public class AddCommentRequest
    {
        [Required]
        public string? Comment { get; set; }
    }
}
