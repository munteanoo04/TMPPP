using MediatR;
using TicketingSystem.Application.DTOs;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Interfaces;

namespace TicketingSystem.Application.Features.Tickets.Commands;

/* 
 * SRP (Single Responsibility Principle): 
 * CreateTicketHandler is responsible ONLY for the orchestration of creating a ticket.
 * 
 * OCP (Open/Closed Principle): 
 * Using MediatR allows adding new cross-cutting concerns (logging, validation, caching) 
 * without modifying this handler.
 */

public record CreateTicketCommand(CreateTicketDto TicketDto) : IRequest<Guid>;

public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTicketHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = new Ticket
        {
            Title = request.TicketDto.Title,
            Description = request.TicketDto.Description,
            Priority = request.TicketDto.Priority,
            CreatedBy = request.TicketDto.CreatedBy
        };

        await _unitOfWork.Tickets.AddAsync(ticket);
        await _unitOfWork.SaveChangesAsync();

        return ticket.Id;
    }
}
