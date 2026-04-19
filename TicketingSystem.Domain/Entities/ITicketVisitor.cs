namespace TicketingSystem.Domain.Entities;

public interface ITicketVisitor
{
    void Visit(BugTicket bug);
    void Visit(FeatureTicket feature);
    void Visit(SupportTicket support);
}
