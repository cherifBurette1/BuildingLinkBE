using FluentValidation;

namespace Application.Features.Drivers.Commands.DeleteDriver
{
    public class DeleteDriverCommandValidator : AbstractValidator<DeleteDriverCommand>
    {
        public DeleteDriverCommandValidator()
        {
            RuleFor(a => a.Id).NotNull().NotEmpty().NotEqual(Guid.Empty).WithMessage("Id Is Required");
        }
    }
}
