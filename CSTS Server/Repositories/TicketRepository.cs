using CSTS.Api.Data;
using CSTS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSTS.Api.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly CstsDbContext _context;

        public TicketRepository(CstsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
        }

        public async Task<Ticket> GetByIdAsync(Guid id)
        {
            return (await _context.Tickets.FindAsync(id))!;
        }

        public async Task<Ticket> GetTicketWithDetailsAsync(Guid id)
        {
            return (await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id))!;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsWithDetailsAsync()
        {
            return await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetUserTicketsWithDetailsAsync(Guid userId)
        {
            return await _context.Tickets
                .Where(t => t.CreatedById == userId)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }
    }
}
