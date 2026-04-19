using TicketingSystem.Domain.Enums;

namespace TicketingSystem.Domain.Entities;

/* 
 * SRP (Single Responsibility Principle): 
 * The Ticket class represents the domain model of a ticket and its state. 
 * Logic regarding Ticket calculations or transitions would live here or in a Domain Service.
 */
public abstract class Ticket : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public string CreatedBy { get; set; } = "Anonymous";
    public string? AssignedTo { get; set; }

    // This method will be implemented by concrete classes (Factory Method products)
    public abstract string GetTicketDetails();

    // Prototype Pattern: Abstract clone method
    public abstract Ticket Clone();

    // Visitor Pattern: Accept visitor
    public abstract void Accept(ITicketVisitor visitor);
}
