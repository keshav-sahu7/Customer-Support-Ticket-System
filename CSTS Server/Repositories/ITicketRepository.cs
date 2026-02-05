using CSTS.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSTS.Api.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket> GetByIdAsync(Guid id);
        Task<IEnumerable<Ticket>> GetTicketsWithDetailsAsync(Guid? userId);
        Task<Ticket> GetTicketWithDetailsAsync(Guid id);
        Task AddAsync(Ticket ticket);
        Task<bool> IsTikcetNumberAvailable(string ticketNumber);
    }
}

