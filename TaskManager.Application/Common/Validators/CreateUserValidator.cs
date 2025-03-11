using FluentValidation;
using TaskManager.Application.Common.Constants;
using TaskManager.Application.Common.DTO;

namespace TaskManager.Application.Common.Validator {
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto> {
        public CreateUserDtoValidator() {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(ErrorMessages.FirstNameRequired)
                .MaximumLength(50).WithMessage(ErrorMessages.FirstNameMaxLength);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(ErrorMessages.LastNameRequired)
                .MaximumLength(50).WithMessage(ErrorMessages.LastNameMaxLength);

            RuleFor(x => x.Email)
             .NotEmpty().WithMessage(ErrorMessages.EmailRequired)
             .EmailAddress().WithMessage(ErrorMessages.EmailInvalid);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ErrorMessages.PasswordRequired)
                .MinimumLength(8).WithMessage(ErrorMessages.PasswordMinLength);

            RuleFor(x => x.Phone)
                .Matches(@"^\d{10}$").WithMessage(ErrorMessages.PhoneInvalid);
        }
    }
}
