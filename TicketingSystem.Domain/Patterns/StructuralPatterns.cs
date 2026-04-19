using TicketingSystem.Domain.Entities;
using System.Collections.Concurrent;

namespace TicketingSystem.Domain.Patterns;

// ============================================================================
// STRUCTURAL PATTERN 1: BRIDGE
// ============================================================================

/*
 * Bridge Pattern:
 * Decouple an abstraction from its implementation so that the two can vary independently.
 */

// Implementation Interface
public interface INotificationChannel
{
    void Send(string message);
}

// Concrete Implementations
public class EmailChannel : INotificationChannel
{
    public void Send(string message) => Console.WriteLine($"[Bridge] Sending Email: {message}");
}

public class SlackChannel : INotificationChannel
{
    public void Send(string message) => Console.WriteLine($"[Bridge] Posting to Slack: {message}");
}

// Abstraction
public abstract class Notification
{
    protected INotificationChannel _channel;
    public Notification(INotificationChannel channel) => _channel = channel;
    public abstract void Notify(string message);
}

// Refined Abstractions
public class UrgentNotification : Notification
{
    public UrgentNotification(INotificationChannel channel) : base(channel) { }
    public override void Notify(string message) => _channel.Send($"!!! URGENT !!! {message}");
}


// ============================================================================
// STRUCTURAL PATTERN 2: DECORATOR
// ============================================================================

/*
 * Decorator Pattern:
 * Attach additional responsibilities to an object dynamically.
 * Provides a flexible alternative to subclassing for extending functionality.
 */

public interface ITicketProcessor
{
    void Process(Ticket ticket);
}

public class BasicTicketProcessor : ITicketProcessor
{
    public void Process(Ticket ticket) => Console.WriteLine($"[Decorator] Processing ticket: {ticket.Title}");
}

public abstract class TicketProcessorDecorator : ITicketProcessor
{
    protected ITicketProcessor _inner;
    public TicketProcessorDecorator(ITicketProcessor inner) => _inner = inner;
    public virtual void Process(Ticket ticket) => _inner.Process(ticket);
}

public class LoggingDecorator : TicketProcessorDecorator
{
    public LoggingDecorator(ITicketProcessor inner) : base(inner) { }
    public override void Process(Ticket ticket)
    {
        Console.WriteLine($"[Decorator] LOG: Starting processing for {ticket.Id}");
        base.Process(ticket);
        Console.WriteLine($"[Decorator] LOG: Finished processing for {ticket.Id}");
    }
}


// ============================================================================
// STRUCTURAL PATTERN 3: PROXY
// ============================================================================

/*
 * Proxy Pattern:
 * Provide a surrogate or placeholder for another object to control access to it.
 */

public interface ISecureDataService
{
    string GetData(string userRole);
}

public class RealDataService : ISecureDataService
{
    public string GetData(string userRole) => "Sensitive Ticket Data Content";
}

public class SecurityProxy : ISecureDataService
{
    private readonly RealDataService _realService = new();
    
    public string GetData(string userRole)
    {
        if (userRole != "Admin")
            return "[Proxy] Access Denied: Admin role required.";
        
        return _realService.GetData(userRole);
    }
}


// ============================================================================
// STRUCTURAL PATTERN 4: FLYWEIGHT
// ============================================================================

/*
 * Flyweight Pattern:
 * Use sharing to support large numbers of fine-grained objects efficiently.
 */

public class StatusIcon
{
    public string Color { get; init; } = string.Empty;
    public string Shape { get; init; } = string.Empty;
    
    public void Draw(int x, int y) => Console.WriteLine($"[Flyweight] Drawing {Color} {Shape} icon at {x},{y}");
}

public class IconFactory
{
    private readonly ConcurrentDictionary<string, StatusIcon> _icons = new();

    public StatusIcon GetIcon(string color, string shape)
    {
        string key = $"{color}_{shape}";
        return _icons.GetOrAdd(key, _ => new StatusIcon { Color = color, Shape = shape });
    }
    
    public int GetTotalIconsCreated() => _icons.Count;
}
