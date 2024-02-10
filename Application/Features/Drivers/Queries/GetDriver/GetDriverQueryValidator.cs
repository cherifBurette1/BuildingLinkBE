using FluentValidation;

namespace Application.Features.Drivers.Queries.GetDriver
{
    public class GetDriverQueryValidator : AbstractValidator<GetDriverQuery>
    {
        public GetDriverQueryValidator()
        {
            RuleFor(a => a.Id).NotNull().NotEmpty().NotEqual(Guid.Empty).WithMessage("Id Is Required");
        }
    }
}
