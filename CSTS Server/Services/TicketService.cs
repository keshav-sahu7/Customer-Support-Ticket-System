using CSTS.Api.Data.Entities;
using CSTS.Api.Dtos;
using CSTS.Api.Repositories;
using CSTS.Api.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSTS.Api.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketRepository _ticketRepository;


        public TicketService(
            IUnitOfWork unitOfWork,
            ITicketRepository ticketRepository)
        {
            _unitOfWork = unitOfWork;
            _ticketRepository = ticketRepository;
        }

        public async Task<Ticket> GetTicketAsync(Guid id)
        {
            return await _ticketRepository.GetTicketWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync(string role, Guid userId)
        {
            if (role == "Admin")
            {
                return await _ticketRepository.GetTicketsWithDetailsAsync();
            }
            else
            {
                return await _ticketRepository.GetUserTicketsWithDetailsAsync(userId);
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

            await _ticketRepository.AddAsync(ticket);
            await _unitOfWork.Commit();

            return ticket;
        }

        public async Task<Ticket> UpdateTicketStatusAsync(Guid id, string newStatusString, Guid userId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

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

            _unitOfWork.Update(ticket);
            _unitOfWork.Add(statusHistory);
            await _unitOfWork.Commit();

            return ticket;
        }

        public async Task<Ticket> AssignTicketAsync(Guid id, Guid assignToId, Guid userId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

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

            _unitOfWork.Update(ticket);
            _unitOfWork.Add(assignmentHistory);
            await _unitOfWork.Commit();

            return ticket;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid id)
        {
            return await _unitOfWork.GetAll<TicketComment>()
                .Include(c => c.CreatedBy)
                .Where(c => c.TicketId == id)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    CreatedByUsername = c.CreatedBy != null ? c.CreatedBy.Username : "Unknown"
                })
                .ToListAsync();
        }

        public async Task<TicketComment> AddCommentAsync(Guid id, string comment, Guid userId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

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

            _unitOfWork.Add(newComment);
            await _unitOfWork.Commit();

            return newComment;
        }
    }
}
