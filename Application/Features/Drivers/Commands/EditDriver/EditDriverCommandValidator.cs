using FluentValidation;

namespace Application.Features.Drivers.Commands.EditDriver
{
    public class DeleteDriverCommandValidator : AbstractValidator<EditDriverCommand>
    {
        public DeleteDriverCommandValidator()
        {
            RuleFor(a => a.Id).NotNull().NotEmpty().NotEqual(Guid.Empty).WithMessage("Id Is Required");
            RuleFor(a => a.Email).NotNull().NotEmpty().WithMessage("Email Address Is Required");
            RuleFor(a => a.FirstName).NotNull().NotEmpty().WithMessage("First Name Is Required");
            RuleFor(a => a.LastName).NotNull().NotEmpty().WithMessage("Last Name Is Required");
        }
    }
}
