using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketingSystem.Domain.Interfaces;
using TicketingSystem.Infrastructure.Data;
using TicketingSystem.Infrastructure.Repositories;

namespace TicketingSystem.Infrastructure;

/* 
 * DIP (Dependency Inversion Principle): 
 * Infrastructure details (SQLite, EF Core) are hidden from the upper layers. 
 * The Application layer only knows about the interfaces.
 */
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}
