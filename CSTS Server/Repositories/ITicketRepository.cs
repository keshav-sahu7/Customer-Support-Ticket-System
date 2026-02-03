using CSTS.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSTS.Api.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket> GetByIdAsync(Guid id);
        Task<IEnumerable<Ticket>> GetTicketsWithDetailsAsync();
        Task<Ticket> GetTicketWithDetailsAsync(Guid id);
        Task<IEnumerable<Ticket>> GetUserTicketsWithDetailsAsync(Guid userId);
        Task AddAsync(Ticket ticket);
    }
}

