using CSTS.Api.Data.Entities;
using CSTS.Api.Dtos;
using CSTS.Api.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSTS.Api.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Ticket> GetTicketAsync(Guid id)
        {
            return await _unitOfWork.Tickets.GetTicketWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync(string role, Guid userId)
        {
            if (role == "Admin")
            {
                return await _unitOfWork.Tickets.GetTicketsWithDetailsAsync();
            }
            else
            {
                return await _unitOfWork.Tickets.GetUserTicketsWithDetailsAsync(userId);
            }
        }

        public async Task<Ticket> CreateTicketAsync(CreateTicketRequest createTicketRequest, Guid userId)
        {
            if (string.IsNullOrEmpty(createTicketRequest.Subject) || string.IsNullOrEmpty(createTicketRequest.Description) || string.IsNullOrEmpty(createTicketRequest.Priority))
            {
                throw new ArgumentException("Subject, description and priority are required.");
            }

            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                Subject = createTicketRequest.Subject,
                Description = createTicketRequest.Description,
                Priority = Enum.Parse<TicketPriority>(createTicketRequest.Priority, true),
                Status = TicketStatus.Open,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId
            };

            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.CompleteAsync();

            return ticket;
        }

        public async Task<Ticket> UpdateTicketStatusAsync(Guid id, string newStatusString, Guid userId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);

            if (ticket == null)
            {
                return null!;
            }

            if (ticket.Status == TicketStatus.Closed)
            {
                throw new Exception("Cannot update a closed ticket.");
            }

            var newStatus = Enum.Parse<TicketStatus>(newStatusString, true);

            if (ticket.Status == TicketStatus.Open && newStatus != TicketStatus.InProgress)
            {
                throw new Exception("A ticket can only be moved from Open to InProgress.");
            }

            if (ticket.Status == TicketStatus.InProgress && newStatus != TicketStatus.Closed)
            {
                throw new Exception("A ticket can only be moved from InProgress to Closed.");
            }

            var statusHistory = new TicketStatusHistory
            {
                Id = Guid.NewGuid(),
                TicketId = ticket.Id,
                OldStatus = ticket.Status,
                NewStatus = newStatus,
                ChangedAt = DateTime.UtcNow,
                ChangedById = userId
            };

            ticket.Status = newStatus;

            await _unitOfWork.Repository<TicketStatusHistory>().AddAsync(statusHistory);
            await _unitOfWork.CompleteAsync();

            return ticket;
        }

        public async Task<Ticket> AssignTicketAsync(Guid id, Guid assignToId, Guid userId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);

            if (ticket == null)
            {
                return null!;
            }

            if (ticket.Status == TicketStatus.Closed)
            {
                throw new Exception("Cannot assign a closed ticket.");
            }

            var oldAssignedToId = ticket.AssignedToId;

            var assignmentHistory = new TicketAssignmentHistory
            {
                Id = Guid.NewGuid(),
                TicketId = ticket.Id,
                OldAssignedToId = oldAssignedToId,
                NewAssignedToId = assignToId,
                ChangedAt = DateTime.UtcNow,
                ChangedById = userId
            };

            ticket.AssignedToId = assignToId;

            await _unitOfWork.Repository<TicketAssignmentHistory>().AddAsync(assignmentHistory);
            await _unitOfWork.CompleteAsync();

            return ticket;
        }

        public async Task<IEnumerable<TicketComment>> GetCommentsAsync(Guid id)
        {
            return await _unitOfWork.Repository<TicketComment>().FindAsync(c => c.TicketId == id);
        }

        public async Task<TicketComment> AddCommentAsync(Guid id, string comment, Guid userId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);

            if (ticket == null)
            {
                return null!;
            }

            if (ticket.Status == TicketStatus.Closed)
            {
                throw new Exception("Cannot add comments to a closed ticket.");
            }

            var newComment = new TicketComment
            {
                Id = Guid.NewGuid(),
                Content = comment,
                CreatedAt = DateTime.UtcNow,
                TicketId = id,
                CreatedById = userId
            };

            await _unitOfWork.Repository<TicketComment>().AddAsync(newComment);
            await _unitOfWork.CompleteAsync();

            return newComment;
        }
    }
}
