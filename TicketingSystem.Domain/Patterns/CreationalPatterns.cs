using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Enums;
using System.Text;

namespace TicketingSystem.Domain.Patterns;

// ============================================================================
// PATTERN 1: FACTORY METHOD
// ============================================================================

/*
 * Factory Method Pattern:
 * Define an interface for creating an object, but let subclasses decide which class to instantiate.
 * Factory Method lets a class defer instantiation to subclasses.
 */

// Concrete Tickets have been moved to TicketingSystem.Domain.Entities to resolve circular dependencies.


// --- Creator (The Factory Method structure) ---

public abstract class TicketCreator
{
    // The Factory Method
    public abstract Ticket CreateTicket();

    // Some operation that uses the created product
    public string ProcessTicketInformation()
    {
        var ticket = CreateTicket();
        return $"Processing ticket tracking ID {ticket.Id}: {ticket.GetTicketDetails()}";
    }
}

// --- Concrete Creators ---

public class BugTicketCreator : TicketCreator
{
    public override Ticket CreateTicket() => new BugTicket 
    { 
        Title = "New Bug Found", 
        Priority = TicketPriority.High,
        StepsToReproduce = "No steps provided yet."
    };
}

public class FeatureRequestCreator : TicketCreator
{
    public override Ticket CreateTicket() => new FeatureTicket 
    { 
        Title = "New Feature Request", 
        Priority = TicketPriority.Low,
        ExpectedImpact = "UX improvement"
    };
}


// ============================================================================
// PATTERN 2: ABSTRACT FACTORY
// ============================================================================

/*
 * Abstract Factory Pattern:
 * Provide an interface for creating families of related or dependent objects 
 * without specifying their concrete classes.
 */

// --- Abstract Products ---

public interface IEmailService
{
    string SendEmail(string recipient, string message);
}

public interface ISmsService
{
    string SendSms(string phoneNumber, string message);
}

// --- Concrete Products for "Standard" family ---

public class StandardEmailService : IEmailService
{
    public string SendEmail(string recipient, string message) => $"Standard SMTP: Sending '{message}' to {recipient}";
}

public class StandardSmsService : ISmsService
{
    public string SendSms(string phoneNumber, string message) => $"Simulated SMS Gateway: Sending '{message}' to {phoneNumber}";
}

// --- Concrete Products for "Premium/Secure" family ---

public class EncyptedEmailService : IEmailService
{
    public string SendEmail(string recipient, string message) => $"Secure Mail Server (PGP): Sending encrypted payload to {recipient}";
}

public class SecureSmsService : ISmsService
{
    public string SendSms(string phoneNumber, string message) => $"Protected SMS API (Twilio): Sending secure message to {phoneNumber}";
}

// --- Abstract Factory ---

public interface INotificationFactory
{
    IEmailService CreateEmailService();
    ISmsService CreateSmsService();
}

// --- Concrete Factories ---

public class StandardNotificationFactory : INotificationFactory
{
    public IEmailService CreateEmailService() => new StandardEmailService();
    public ISmsService CreateSmsService() => new StandardSmsService();
}

public class SecureNotificationFactory : INotificationFactory
{
    public IEmailService CreateEmailService() => new EncyptedEmailService();
    public ISmsService CreateSmsService() => new SecureSmsService();
}


// ============================================================================
// PATTERN 3: SINGLETON
// ============================================================================

/*
 * Singleton Pattern:
 * Ensure a class only has one instance and provide a global point of access to it.
 */

public sealed class SystemConfigurationManager
{
    private static readonly Lazy<SystemConfigurationManager> _instance = 
        new(() => new SystemConfigurationManager());

    public static SystemConfigurationManager Instance => _instance.Value;

    private SystemConfigurationManager()
    {
        SystemName = "Ticketing Master 3000";
        MaxTicketsPerUser = 50;
        AllowedDomains = new List<string> { "company.com", "partner.net" };
    }

    public string SystemName { get; init; }
    public int MaxTicketsPerUser { get; set; }
    public List<string> AllowedDomains { get; private set; }

    public void LogCurrentConfig()
    {
        Console.WriteLine($"[Singleton] System: {SystemName} | Limit: {MaxTicketsPerUser}");
    }
}


// ============================================================================
// PATTERN 4: BUILDER
// ============================================================================

/*
 * Builder Pattern:
 * Separate the construction of a complex object from its representation.
 */

public class TicketReport
{
    public string Title { get; set; } = "Untitled Report";
    public string Header { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Footer { get; set; } = string.Empty;

    public void Display()
    {
        Console.WriteLine("\n--- TICKET REPORT ---");
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Header: {Header}");
        Console.WriteLine("Content:\n" + Content);
        Console.WriteLine($"Footer: {Footer}");
        Console.WriteLine("---------------------\n");
    }
}

public interface ITicketReportBuilder
{
    ITicketReportBuilder SetTitle(string title);
    ITicketReportBuilder SetHeader(string systemName);
    ITicketReportBuilder SetContent(IEnumerable<Ticket> tickets);
    ITicketReportBuilder SetFooter();
    TicketReport Build();
}

public class ModernReportBuilder : ITicketReportBuilder
{
    private TicketReport _report = new();

    public ITicketReportBuilder SetTitle(string title) { _report.Title = title.ToUpper(); return this; }
    public ITicketReportBuilder SetHeader(string systemName) { _report.Header = $"Generated by {systemName} on {DateTime.Now:yyyy-MM-dd HH:mm}"; return this; }
    public ITicketReportBuilder SetContent(IEnumerable<Ticket> tickets)
    {
        var sb = new StringBuilder();
        foreach (var t in tickets) sb.AppendLine($"- [{t.Priority}] {t.Title} ({t.Status})");
        _report.Content = sb.ToString();
        return this;
    }
    public ITicketReportBuilder SetFooter() { _report.Footer = "Contact: support@ticketing-master.com"; return this; }
    public TicketReport Build() { var result = _report; _report = new TicketReport(); return result; }
}

public class ReportDirector
{
    private readonly ITicketReportBuilder _builder;
    public ReportDirector(ITicketReportBuilder builder) => _builder = builder;
    public TicketReport ConstructFullReport(string title, IEnumerable<Ticket> tickets)
    {
        return _builder.SetTitle(title).SetHeader(SystemConfigurationManager.Instance.SystemName).SetContent(tickets).SetFooter().Build();
    }
}
