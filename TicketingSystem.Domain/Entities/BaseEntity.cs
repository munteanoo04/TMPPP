using System;

namespace TicketingSystem.Domain.Entities;

/* 
 * SRP (Single Responsibility Principle): 
 * This base class is only responsible for defining core entity properties shared across the system.
 */
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
