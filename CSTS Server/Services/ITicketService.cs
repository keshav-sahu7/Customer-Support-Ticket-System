using CSTS.Api.Data.Entities;
using CSTS.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSTS.Api.Services
{
    public interface ITicketService
    {
        Task<Ticket> GetTicketAsync(Guid id);
        Task<IEnumerable<Ticket>> GetTicketsAsync(string role, Guid userId);
        Task<Ticket> CreateTicketAsync(CreateTicketRequest createTicketRequest, Guid userId);
        Task<Ticket> UpdateTicketStatusAsync(Guid id, string newStatus, Guid userId);
        Task<Ticket> AssignTicketAsync(Guid id, Guid assignToId, Guid userId);
        Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid id);
        Task<TicketComment> AddCommentAsync(Guid id, string comment, Guid userId);
    }
}
