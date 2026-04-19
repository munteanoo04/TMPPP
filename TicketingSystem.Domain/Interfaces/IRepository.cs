using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketingSystem.Domain.Interfaces;

/* 
 * ISP (Interface Segregation Principle): 
 * We define generic interfaces and specific ones to ensure clients aren't forced to depend on methods they don't use.
 * 
 * DIP (Dependency Inversion Principle): 
 * High-level business logic in the Application layer depends on these abstractions, 
 * while low-level Infrastructure implementations depend on these same abstractions.
 */

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}

public interface ITicketRepository : IRepository<Entities.Ticket>
{
    // Specific methods for Tickets can be added here
    Task<IEnumerable<Entities.Ticket>> GetTicketsByStatusAsync(Enums.TicketStatus status);
}

public interface IUnitOfWork : IDisposable
{
    ITicketRepository Tickets { get; }
    Task<int> SaveChangesAsync();
}
