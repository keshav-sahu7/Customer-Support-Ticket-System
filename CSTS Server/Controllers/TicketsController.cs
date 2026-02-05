using CSTS.Api.Data.Entities;
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

        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetTickets([FromQuery] string role, Guid userId)
        {
            try
            {
                var tickets = await _ticketService.GetTicketsAsync(userId,  Enum.Parse<UserRole>(role));
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
                var ticket = await _ticketService.CreateTicketAsync(createTicketRequest);
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{role}")]
        public async Task<IActionResult> UpdateTicket(string role, [FromBody] UpdateTicketDetailsRequest request)
        {
            try
            {
                if (Enum.Parse<UserRole>(role) != UserRole.Admin)
                {
                    throw new InvalidOperationException("Only admin allowed to change ticket");
                }
                
                var ticket = await _ticketService.UpdateTicketDetailsAsync(request);
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
