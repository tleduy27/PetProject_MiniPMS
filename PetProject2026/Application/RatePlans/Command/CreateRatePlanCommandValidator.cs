using FluentValidation;

namespace PetProject2026.Application.RatePlans.Command
{
    public class CreateRatePlanCommandValidator : AbstractValidator<CreateRatePlanCommand>
    {
        public CreateRatePlanCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống").MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
      
        }
    }
}
