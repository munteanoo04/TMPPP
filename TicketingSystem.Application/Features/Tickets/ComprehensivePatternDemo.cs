using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Patterns;
using TicketingSystem.Domain.Enums;

namespace TicketingSystem.Application.Features.Tickets;

public class ComprehensivePatternDemo
{
    public void ExecuteAllDemos()
    {
        Console.WriteLine("\n=== COMPREHENSIVE DESIGN PATTERN DEMO ===\n");

        // --- STRUCTURAL ---
        
        // 1. Bridge
        var urgentSlack = new UrgentNotification(new SlackChannel());
        urgentSlack.Notify("Server Down!");

        // 2. Decorator
        ITicketProcessor processor = new LoggingDecorator(new BasicTicketProcessor());
        processor.Process(new BugTicket { Title = "Fix typo" });

        // 3. Proxy
        ISecureDataService proxy = new SecurityProxy();
        Console.WriteLine(proxy.GetData("Guest"));
        Console.WriteLine(proxy.GetData("Admin"));

        // 4. Flyweight
        var factory = new IconFactory();
        var icon1 = factory.GetIcon("Red", "Circle");
        var icon2 = factory.GetIcon("Red", "Circle");
        icon1.Draw(10, 10);
        Console.WriteLine($"[Flyweight] Is same reference: {ReferenceEquals(icon1, icon2)}");


        // --- BEHAVIORAL A ---

        // 5. Chain of Responsibility
        var manager = new ManagerApprover();
        var admin = new AdminApprover();
        manager.SetSuccessor(admin);
        manager.ProcessRequest(5); // Should escalate to Admin

        // 6. State
        var ticketContext = new TicketContext(new OpenState());
        ticketContext.Request();
        ticketContext.Request();

        // 7. Mediator
        var c1 = new Component1();
        var c2 = new Component2();
        new SupportMediator(c1, c2);
        c1.DoA();

        // 8. Template Method
        DataExporter exporter = new CsvDataExporter();
        exporter.Export();

        // 9. Visitor
        var audit = new AuditVisitor();
        var bug = new BugTicket { Title = "Memory Leak" };
        bug.Accept(audit); // Requires 'Accept' method in Ticket hierarchy


        // --- BEHAVIORAL B ---

        // 10. Strategy
        IAssignmentStrategy strategy = new RoundRobinStrategy();
        Console.WriteLine($"[Strategy] Assigned to: {strategy.Assign(bug)}");

        // 11. Observer
        var notifier = new TicketNotifier();
        // (Assuming an observer implementation exists)

        // 12. Command
        var closeCmd = new CloseTicketCommand(bug);
        closeCmd.Execute();
        closeCmd.Undo();

        // 13. Memento
        var caretaker = new TicketCaretaker();
        caretaker.Memento = new TicketMemento(bug.Title, bug.Description);
        Console.WriteLine($"[Memento] Saved state for: {caretaker.Memento.Title}");

        // 14. Iterator
        var tickets = new List<Ticket> { bug, new FeatureTicket { Title = "Dark Mode", Priority = TicketPriority.Low } };
        var iterator = new PriorityIterator(tickets);
        while (iterator.MoveNext())
        {
            var t = (Ticket)iterator.Current;
            Console.WriteLine($"[Iterator] Next: {t.Title} ({t.Priority})");
        }
    }
}
