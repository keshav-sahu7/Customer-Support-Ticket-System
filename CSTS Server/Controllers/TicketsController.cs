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
            try
            {
                var ticket = await _ticketService.GetTicketAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets([FromQuery] string role = "Admin")
        {
            try
            {
                var userId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"); // Hardcoded user for now
                var tickets = await _ticketService.GetTicketsAsync(role, userId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest createTicketRequest)
        {
            try
            {
                var userId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"); // Hardcoded user for now
                var ticket = await _ticketService.CreateTicketAsync(createTicketRequest, userId);
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTicketStatus(Guid id, [FromBody] UpdateTicketStatusRequest updateTicketStatusRequest)
        {
            try
            {
                var userId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"); // Hardcoded user for now
                var ticket = await _ticketService.UpdateTicketStatusAsync(id, updateTicketStatusRequest.Status, userId);
                if (ticket == null)
                {
                    return NotFound();
                }
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/assign")]
        public async Task<IActionResult> AssignTicket(Guid id, [FromBody] AssignTicketRequest assignTicketRequest)
        {
            try
            {
                var userId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"); // Hardcoded user for now
                var ticket = await _ticketService.AssignTicketAsync(id, assignTicketRequest.AssignToId, userId);
                if (ticket == null)
                {
                    return NotFound();
                }
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
