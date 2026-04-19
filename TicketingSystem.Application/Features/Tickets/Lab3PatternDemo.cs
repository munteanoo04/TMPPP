using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Patterns;

namespace TicketingSystem.Application.Features.Tickets;

/*
 * Lab 3 - Creational Patterns Demonstration
 * This class shows how the implemented patterns are used in a real application logic.
 */
public class Lab3PatternDemo
{
    public void RunDesignPatternDemo()
    {
        // 1. SINGLETON DEMO
        // Accessing global configuration
        var config = SystemConfigurationManager.Instance;
        Console.WriteLine($"[Singleton] Accessing system: {config.SystemName}");
        config.LogCurrentConfig();


        // 2. PROTOTYPE DEMO
        // Cloning an existing ticket to create a recurring one
        var originalTicket = new BugTicket 
        { 
            Title = "CRITICAL: Database connection leak", 
            Description = "Pool exhausted under high load",
            StepsToReproduce = "Run 1000 concurrent queries"
        };
        
        // Use the Prototype pattern (Clone)
        var duplicatedTicket = (BugTicket)originalTicket.Clone();
        duplicatedTicket.Title = "[CLONE] " + originalTicket.Title;
        
        Console.WriteLine($"[Prototype] Original: {originalTicket.Title}");
        Console.WriteLine($"[Prototype] Duplicated: {duplicatedTicket.Title}");


        // 3. BUILDER DEMO
        // Building a complex report from a list of tickets
        var tickets = new List<Ticket> { originalTicket, duplicatedTicket };
        
        // Instantiate the builder and director
        ITicketReportBuilder builder = new ModernReportBuilder();
        ReportDirector director = new ReportDirector(builder);
        
        // The director orchestrates the building process
        var report = director.ConstructFullReport("System Integrity Audit", tickets);
        
        // Show the built product
        Console.WriteLine("[Builder] Complex report generated:");
        report.Display();
    }
}
