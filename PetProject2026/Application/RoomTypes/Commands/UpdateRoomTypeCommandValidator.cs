using FluentValidation;

namespace PetProject2026.Application.RoomTypes.Commands
{
    public class UpdateRoomTypeCommandValidator : AbstractValidator<UpdateRoomTypeCommand>
    {
        public UpdateRoomTypeCommandValidator() { 
        RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên loại phòng không được để trống")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.MaxOccupancy).GreaterThan(0).WithMessage("Số người tối đa phải lớn hơn 0");
            RuleFor(x => x.BasePrice).GreaterThanOrEqualTo(0).WithMessage("Giá không được âm");
        }
    }
}
