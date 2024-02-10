using FluentValidation;

namespace Application.Features.Drivers.Commands.CreateDriver
{
    public class EditDriverCommandValidator : AbstractValidator<CreateDriverCommand>
    {
        public EditDriverCommandValidator()
        {
            RuleFor(a => a.Email).NotNull().NotEmpty().WithMessage("Email Address Is Required");
            RuleFor(a => a.FirstName).NotNull().NotEmpty().WithMessage("First Name Is Required");
            RuleFor(a => a.LastName).NotNull().NotEmpty().WithMessage("Last Name Is Required");
        }
    }
}
