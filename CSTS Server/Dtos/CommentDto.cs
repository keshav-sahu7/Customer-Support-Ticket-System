using System;

namespace CSTS.Api.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatedByUsername { get; set; } = string.Empty;
    }
}
