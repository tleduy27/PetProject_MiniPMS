using FluentValidation;

namespace PetProject2026.Application.RoomRates.Command
{
    public class CreateRoomRateCommandValidator : AbstractValidator<CreateRoomRateCommand>
    {
        public CreateRoomRateCommandValidator()
        {
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price phải lớn hơn 0");
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("EndDate phải sau StartDate");
        }
}
}
