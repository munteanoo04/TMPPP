using TicketingSystem.Domain.Entities;
using System.Collections;

namespace TicketingSystem.Domain.Patterns;

// ============================================================================
// BEHAVIORAL PATTERN 1: CHAIN OF RESPONSIBILITY
// ============================================================================

public abstract class Approver
{
    protected Approver? Successor;
    public void SetSuccessor(Approver successor) => Successor = successor;
    public abstract void ProcessRequest(int priorityLevel);
}

public class ManagerApprover : Approver
{
    public override void ProcessRequest(int priorityLevel)
    {
        if (priorityLevel < 3) Console.WriteLine("[CoR] Manager approved the request.");
        else Successor?.ProcessRequest(priorityLevel);
    }
}

public class AdminApprover : Approver
{
    public override void ProcessRequest(int priorityLevel) => Console.WriteLine("[CoR] Admin Escalation: Request approved.");
}


// ============================================================================
// BEHAVIORAL PATTERN 2: STATE
// ============================================================================

public interface ITicketState { void Handle(TicketContext context); }

public class TicketContext
{
    public ITicketState State { get; set; }
    public TicketContext(ITicketState state) => State = state;
    public void Request() => State.Handle(this);
}

public class OpenState : ITicketState
{
    public void Handle(TicketContext context) { Console.WriteLine("[State] Ticket is OPEN. Moving to IN PROGRESS."); context.State = new InProgressState(); }
}

public class InProgressState : ITicketState
{
    public void Handle(TicketContext context) { Console.WriteLine("[State] Ticket is IN PROGRESS. Resolving."); context.State = new ResolvedState(); }
}

public class ResolvedState : ITicketState { public void Handle(TicketContext c) => Console.WriteLine("[State] Ticket is RESOLVED."); }


// ============================================================================
// BEHAVIORAL PATTERN 3: MEDIATOR
// ============================================================================

public interface IMediator { void Notify(object sender, string ev); }

public class SupportMediator : IMediator
{
    private Component1 _c1;
    private Component2 _c2;
    public SupportMediator(Component1 c1, Component2 c2) { _c1 = c1; _c1.SetMediator(this); _c2 = c2; _c2.SetMediator(this); }
    public void Notify(object sender, string ev) { if (ev == "A") { Console.WriteLine("[Mediator] A triggered -> B."); _c2.DoD(); } }
}

public class Component1 { protected IMediator _m; public void SetMediator(IMediator m) => _m = m; public void DoA() => _m.Notify(this, "A"); }
public class Component2 { protected IMediator _m; public void SetMediator(IMediator m) => _m = m; public void DoD() => Console.WriteLine("[Mediator] D performed."); }


// ============================================================================
// BEHAVIORAL PATTERN 4: TEMPLATE METHOD
// ============================================================================

public abstract class DataExporter
{
    public void Export() { Connect(); FormatData(); Upload(); }
    protected void Connect() => Console.WriteLine("[Template] Connecting...");
    protected abstract void FormatData();
    protected void Upload() => Console.WriteLine("[Template] Uploading...");
}

public class CsvDataExporter : DataExporter { protected override void FormatData() => Console.WriteLine("[Template] Formatting as CSV."); }


// ============================================================================
// BEHAVIORAL PATTERN 5: VISITOR
// ============================================================================

// ITicketVisitor has been moved to TicketingSystem.Domain.Entities to resolve circular dependencies.

public class AuditVisitor : ITicketVisitor
{
    public void Visit(BugTicket bug) => Console.WriteLine($"[Visitor] Auditing BUG: {bug.Title}");
    public void Visit(FeatureTicket feature) => Console.WriteLine($"[Visitor] Auditing FEATURE: {feature.Title}");
    public void Visit(SupportTicket support) => Console.WriteLine($"[Visitor] Auditing SUPPORT: {support.Title}");
}


// ============================================================================
// BEHAVIORAL PATTERN 6: STRATEGY
// ============================================================================

public interface IAssignmentStrategy { string Assign(Ticket ticket); }
public class RoundRobinStrategy : IAssignmentStrategy { public string Assign(Ticket ticket) => "Support-Agent-A (Round Robin)"; }
public class RandomAssignmentStrategy : IAssignmentStrategy { public string Assign(Ticket ticket) => "Support-Agent-B (Random)"; }


// ============================================================================
// BEHAVIORAL PATTERN 7: OBSERVER
// ============================================================================

public interface ITicketObserver { void Update(string message); }
public class TicketNotifier
{
    private List<ITicketObserver> _observers = new();
    public void Attach(ITicketObserver o) => _observers.Add(o);
    public void Notify(string message) => _observers.ForEach(o => o.Update(message));
}


// ============================================================================
// BEHAVIORAL PATTERN 8: COMMAND
// ============================================================================

public interface ICommand { void Execute(); void Undo(); }
public class CloseTicketCommand : ICommand
{
    private Ticket _ticket;
    public CloseTicketCommand(Ticket ticket) => _ticket = ticket;
    public void Execute() { _ticket.Status = TicketingSystem.Domain.Enums.TicketStatus.Closed; Console.WriteLine("[Command] CLOSED."); }
    public void Undo() { _ticket.Status = TicketingSystem.Domain.Enums.TicketStatus.Open; Console.WriteLine("[Command] REOPENED."); }
}


// ============================================================================
// BEHAVIORAL PATTERN 9: MEMENTO
// ============================================================================

public class TicketMemento
{
    public string Title { get; }
    public string Description { get; }
    public TicketMemento(string title, string desc) { Title = title; Description = desc; }
}

public class TicketCaretaker { public TicketMemento? Memento { get; set; } }


// ============================================================================
// BEHAVIORAL PATTERN 10: ITERATOR
// ============================================================================

public class PriorityIterator : IEnumerator
{
    private List<Ticket> _tickets;
    private int _position = -1;
    public PriorityIterator(List<Ticket> tickets) => _tickets = tickets.OrderByDescending(t => t.Priority).ToList();
    public object Current => _tickets[_position];
    public bool MoveNext() => ++_position < _tickets.Count;
    public void Reset() => _position = -1;
}
