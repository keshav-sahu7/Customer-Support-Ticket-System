using CSTS.Api.Data.Entities;
using CSTS.Api.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSTS.Api.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task AddAsync(Ticket ticket)
        {
            _unitOfWork.Add(ticket);
            return Task.CompletedTask;
        }

        public async Task<Ticket> GetByIdAsync(Guid id)
        {
            return (await _unitOfWork.GetAll<Ticket>().FirstOrDefaultAsync(t => t.Id == id))!;
        }

        public async Task<Ticket> GetTicketWithDetailsAsync(Guid id)
        {
            return (await _unitOfWork.GetAll<Ticket>()
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id))!;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsWithDetailsAsync()
        {
            return await _unitOfWork.GetAll<Ticket>()
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetUserTicketsWithDetailsAsync(Guid userId)
        {
            return await _unitOfWork.GetAll<Ticket>()
                .Where(t => t.CreatedById == userId)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }
    }
}
