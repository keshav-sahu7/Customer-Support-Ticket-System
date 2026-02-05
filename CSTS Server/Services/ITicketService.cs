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
        Task<IEnumerable<Ticket>> GetTicketsAsync(Guid userId, UserRole role);
        Task<Ticket> CreateTicketAsync(CreateTicketRequest createTicketRequest);
        Task<Ticket> UpdateTicketDetailsAsync(UpdateTicketDetailsRequest request);
        Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid id);
        Task<TicketComment> AddCommentAsync(Guid id, string comment, Guid userId);
    }
}
