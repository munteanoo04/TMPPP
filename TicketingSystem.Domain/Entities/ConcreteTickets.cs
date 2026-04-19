using TicketingSystem.Domain.Enums;

namespace TicketingSystem.Domain.Entities;

// --- Concrete Products (Implementations of abstract Ticket) ---

public class BugTicket : Ticket
{
    public string StepsToReproduce { get; set; } = string.Empty;
    public override string GetTicketDetails() => $"[BUG] {Title}: {Description} (Steps: {StepsToReproduce})";
    
    // Prototype implementation
    public override Ticket Clone() => (Ticket)this.MemberwiseClone();

    // Visitor implementation
    public override void Accept(ITicketVisitor visitor) => visitor.Visit(this);
}

public class FeatureTicket : Ticket
{
    public string ExpectedImpact { get; set; } = string.Empty;
    public override string GetTicketDetails() => $"[FEATURE] {Title}: {Description} (Impact: {ExpectedImpact})";

    // Prototype implementation
    public override Ticket Clone() => (Ticket)this.MemberwiseClone();

    // Visitor implementation
    public override void Accept(ITicketVisitor visitor) => visitor.Visit(this);
}

public class SupportTicket : Ticket
{
    public string CustomerContact { get; set; } = string.Empty;
    public override string GetTicketDetails() => $"[SUPPORT] {Title}: {Description} (Contact: {CustomerContact})";

    // Prototype implementation
    public override Ticket Clone() => (Ticket)this.MemberwiseClone();

    // Visitor implementation
    public override void Accept(ITicketVisitor visitor) => visitor.Visit(this);
}
