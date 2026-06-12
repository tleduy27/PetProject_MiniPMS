using FluentValidation;

namespace PetProject2026.Application.Guests.Command
{
    public class CreateGuestCommandValidator : AbstractValidator<CreateGuestCommand>
    {
        public CreateGuestCommandValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Họ tên không được để trống").MaximumLength(150);
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email không hợp lệ").When(x => !string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.Phone).Matches(@"^0\d{9,10}$").WithMessage("Số điện thoại không hợp lệ").When(x => !string.IsNullOrWhiteSpace(x.Phone));
        }
    }
}
