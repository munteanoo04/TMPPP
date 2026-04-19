using System.Threading.Tasks;
using TicketingSystem.Domain.Interfaces;
using TicketingSystem.Infrastructure.Data;

namespace TicketingSystem.Infrastructure.Repositories;

/* 
 * SRP (Single Responsibility Principle): 
 * UnitOfWork is responsible for coordinating the work of multiple repositories and 
 * ensuring data integrity via a single transaction/save operation.
 */
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private ITicketRepository? _tickets;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public ITicketRepository Tickets => _tickets ??= new TicketRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
