using CSTS.Api.Data.Entities;
using CSTS.Api.UnitOfWork;
using LinqKit;
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

        public async Task<IEnumerable<Ticket>> GetTicketsWithDetailsAsync(Guid? userId)
        {
            var predicate = PredicateBuilder.New<Ticket>(true);

            if (userId != null)
                predicate = predicate.And(t => t.CreatedById == userId);

            return await _unitOfWork.GetAll<Ticket>()
                .Where(predicate)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<bool> IsTikcetNumberAvailable(string ticketNumber)
        {
            return !_unitOfWork.GetAll<Ticket>()
                .Any(t => t.TicketNumber == ticketNumber);
        }
    }
}
