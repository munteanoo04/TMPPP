using FluentValidation;
using TicketingSystem.Application.DTOs;

namespace TicketingSystem.Application.Validators;

/* 
 * SRP (Single Responsibility Principle): 
 * This class is responsible ONLY for validating the CreateTicketDto.
 */
public class CreateTicketValidator : AbstractValidator<CreateTicketDto>
{
    public CreateTicketValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.CreatedBy).NotEmpty();
    }
}
