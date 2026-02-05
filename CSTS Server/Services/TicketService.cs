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

        private async Task<string> GenerateUniqueTicketNumberAsync()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string ticketNumber;
            bool isUnique;

            do
            {
                ticketNumber = new string(Enumerable.Repeat(chars, 6)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                // Check for uniqueness in the database
                isUnique = await _ticketRepository.IsTikcetNumberAvailable(ticketNumber);

            } while (!isUnique);

            return ticketNumber;
        }

        public async Task<Ticket> GetTicketAsync(Guid id)
        {
            return await _ticketRepository.GetTicketWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync(Guid userId, UserRole role)
        {

            return await _ticketRepository.GetTicketsWithDetailsAsync(role == UserRole.User ? userId : null);
        }

        public async Task<Ticket> CreateTicketAsync(CreateTicketRequest createTicketRequest)
        {
            if (string.IsNullOrEmpty(createTicketRequest.Subject) || string.IsNullOrEmpty(createTicketRequest.Description) || string.IsNullOrEmpty(createTicketRequest.Priority))
            {
                throw new ArgumentException("Subject, description and priority are required.");
            }

            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                TicketNumber = await GenerateUniqueTicketNumberAsync(),
                Subject = createTicketRequest.Subject,
                Description = createTicketRequest.Description,
                Priority = Enum.Parse<TicketPriority>(createTicketRequest.Priority, true),
                Status = TicketStatus.Open,
                CreatedAt = DateTime.UtcNow,
                CreatedById = createTicketRequest.UserId
            };

            await _ticketRepository.AddAsync(ticket);
            await _unitOfWork.Commit();

            return ticket;
        }

        public async Task<Ticket> UpdateTicketDetailsAsync(UpdateTicketDetailsRequest request)
        {
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId);

            if (ticket == null)
            {
                return null!;
            }

            if (ticket.Status == TicketStatus.Closed)
            {
                throw new Exception("Cannot update a closed ticket.");
            }

            if (request.Status != null)
            {
                var newStatus = Enum.Parse<TicketStatus>(request.Status, true);

                if (ticket.Status != newStatus)
                {
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
                        ChangedById = request.UserId
                    };

                    ticket.Status = newStatus;
                    _unitOfWork.Add(statusHistory);
                }
            }

            if (request.AssigneeId.HasValue && ticket.AssignedToId != request.AssigneeId.Value)
            {
                var oldAssignedToId = ticket.AssignedToId;

                var assignmentHistory = new TicketAssignmentHistory
                {
                    Id = Guid.NewGuid(),
                    TicketId = ticket.Id,
                    OldAssignedToId = oldAssignedToId,
                    NewAssignedToId = request.AssigneeId.Value,
                    ChangedAt = DateTime.UtcNow,
                    ChangedById = request.UserId
                };

                ticket.AssignedToId = request.AssigneeId.Value;
                _unitOfWork.Add(assignmentHistory);
            }
            
            _unitOfWork.Update(ticket);
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
