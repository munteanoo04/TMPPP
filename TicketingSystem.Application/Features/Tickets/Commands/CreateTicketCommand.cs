using MediatR;
using TicketingSystem.Application.DTOs;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Interfaces;
using TicketingSystem.Domain.Patterns; // Import the patterns

namespace TicketingSystem.Application.Features.Tickets.Commands;

/* 
 * SOLID Compliance in Lab 2:
 * 
 * SRP (Single Responsibility Principle): 
 * CreateTicketHandler orchestration remains focused on ticket creation.
 * 
 * OCP (Open/Closed Principle): 
 * We use patterns to extend ticket types and notification families without modifying the core logic.
 * 
 * DIP (Dependency Inversion Principle):
 * The handler uses abstract creators and factories rather than concrete types where possible.
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
        // ============================================================================
        // DEMONSTRATION: FACTORY METHOD
        // Instead of 'new Ticket()', we use a creator that decides the concrete type.
        // ============================================================================
        
        TicketCreator creator;
        if (request.TicketDto.Title.Contains("BUG", StringComparison.OrdinalIgnoreCase))
        {
            creator = new BugTicketCreator();
        }
        else
        {
            creator = new FeatureRequestCreator();
        }

        // The Factory Method creates the object for us
        var ticket = creator.CreateTicket();
        
        // Map common properties
        ticket.Title = request.TicketDto.Title;
        ticket.Description = request.TicketDto.Description;
        ticket.Priority = request.TicketDto.Priority;
        ticket.CreatedBy = request.TicketDto.CreatedBy;

        await _unitOfWork.Tickets.AddAsync(ticket);
        await _unitOfWork.SaveChangesAsync();

        // ============================================================================
        // DEMONSTRATION: ABSTRACT FACTORY
        // We create a family of related products (Email + SMS) for the notification.
        // ============================================================================
        
        // In a real scenario, this factory could be injected based on user preference
        INotificationFactory notificationFactory = new StandardNotificationFactory();
        
        var emailService = notificationFactory.CreateEmailService();
        var smsService = notificationFactory.CreateSmsService();

        // Using the products created by the abstract factory
        emailService.SendEmail(ticket.CreatedBy, $"Ticket {ticket.Id} has been created.");
        smsService.SendSms("0123456789", $"New ticket alert: {ticket.Title}");

        return ticket.Id;
    }
}
