using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Application.DTOs;
using TicketingSystem.Domain.Interfaces;

namespace TicketingSystem.Application.Features.Tickets.Queries;

/* 
 * SRP (Single Responsibility Principle): 
 * GetTicketsHandler is responsible ONLY for retrieving tickets.
 */

public record GetTicketsQuery : IRequest<IEnumerable<TicketDto>>;

public class GetTicketsHandler : IRequestHandler<GetTicketsQuery, IEnumerable<TicketDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTicketsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TicketDto>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _unitOfWork.Tickets.GetAllAsync();
        
        return tickets.Select(t => new TicketDto(
            t.Id,
            t.Title,
            t.Description,
            t.Status,
            t.Priority,
            t.CreatedBy,
            t.AssignedTo,
            t.CreatedAt
        ));
    }
}
