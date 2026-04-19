using TicketingSystem.Domain.Enums;

namespace TicketingSystem.Application.DTOs;

/* 
 * SRP (Single Responsibility Principle): 
 * These DTOs are only responsible for carrying data between layers.
 */

public record TicketDto(
    Guid Id,
    string Title,
    string Description,
    TicketStatus Status,
    TicketPriority Priority,
    string CreatedBy,
    string? AssignedTo,
    DateTime CreatedAt
);

public class CreateTicketDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public string CreatedBy { get; set; } = string.Empty;

    public CreateTicketDto() { }

    public CreateTicketDto(string title, string description, TicketPriority priority, string createdBy)
    {
        Title = title;
        Description = description;
        Priority = priority;
        CreatedBy = createdBy;
    }
}
