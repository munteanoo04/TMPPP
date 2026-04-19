using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystem.Application.DTOs;
using TicketingSystem.Application.Features.Tickets.Commands;
using TicketingSystem.Application.Features.Tickets.Queries;

namespace TicketingSystem.WebAPI.Controllers;

/* 
 * SRP (Single Responsibility Principle): 
 * The API controller is only responsible for handling HTTP requests and 
 * returning responses. It delegates all business logic to MediatR handlers.
 */

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
    {
        var result = await _mediator.Send(new GetTicketsQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTicket(CreateTicketDto ticketDto)
    {
        var result = await _mediator.Send(new CreateTicketCommand(ticketDto));
        return Ok(result);
    }
}
