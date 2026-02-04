using CSTS.Api.Dtos;
using CSTS.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CSTS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(Guid id)
        {
            var ticket = await _ticketService.GetTicketAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets([FromQuery] string role = "Admin")
        {
            var userId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"); // Hardcoded user for now
            var tickets = await _ticketService.GetTicketsAsync(role, userId);
            return Ok(tickets);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest createTicketRequest)
        {
            var userId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"); // Hardcoded user for now
            var ticket = await _ticketService.CreateTicketAsync(createTicketRequest, userId);
            return Ok(ticket);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTicketStatus(Guid id, [FromBody] UpdateTicketStatusRequest updateTicketStatusRequest)
        {
            var userId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"); // Hardcoded user for now
            var ticket = await _ticketService.UpdateTicketStatusAsync(id, updateTicketStatusRequest.Status, userId);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }

        [HttpPut("{id}/assign")]
        public async Task<IActionResult> AssignTicket(Guid id, [FromBody] AssignTicketRequest assignTicketRequest)
        {
            var userId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"); // Hardcoded user for now
            var ticket = await _ticketService.AssignTicketAsync(id, assignTicketRequest.AssignToId, userId);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetComments(Guid id)
        {
            var comments = await _ticketService.GetCommentsAsync(id);
            return Ok(comments);
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(Guid id, [FromBody] AddCommentRequest addCommentRequest)
        {
            var userId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"); // Hardcoded user for now
            var comment = await _ticketService.AddCommentAsync(id, addCommentRequest.Comment, userId);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }
    }
}
