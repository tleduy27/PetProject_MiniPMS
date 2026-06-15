using FluentValidation;
using Microsoft.Identity.Client;

namespace PetProject2026.Application.RoomTypes.Commands
{
    public class CreateRoomTypeCommandValidator : AbstractValidator<CreateRoomTypeCommand>
    {
        public CreateRoomTypeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên loại phòng không được để trống")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.MaxOccupancy).GreaterThan(0).WithMessage("Số người tối đa phải lớn hơn 0");
            RuleFor(x => x.BasePrice).GreaterThanOrEqualTo(0).WithMessage("Giá không được âm");
        }
    }
}
