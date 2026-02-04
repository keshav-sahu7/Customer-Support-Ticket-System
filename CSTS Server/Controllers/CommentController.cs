using CSTS.Api.Dtos;
using CSTS.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSTS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController: ControllerBase
    {
        private readonly ITicketService _ticketService;
        public CommentController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComments(Guid id)
        {
            try
            {
                var comments = await _ticketService.GetCommentsAsync(id);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{userId}/{id}")]
        public async Task<IActionResult> AddComment(Guid id, Guid userId, [FromBody] AddCommentRequest addCommentRequest)
        {
            try
            {
                var comment = await _ticketService.AddCommentAsync(id, addCommentRequest.Comment, userId);
                if (comment == null)
                {
                    return BadRequest("Ticket not found!");
                }
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
