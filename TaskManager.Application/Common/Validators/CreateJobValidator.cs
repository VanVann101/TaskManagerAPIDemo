using FluentValidation;
using TaskManager.Application.Common.Constants;
using TaskManager.Application.Common.DTO;

namespace TaskManager.Application.Common.Validator {
    public class CreateJobValidator : AbstractValidator<CreateJobDto> {
        public CreateJobValidator() {
            RuleFor(job => job.Name)
                .NotEmpty().WithMessage(ErrorMessages.TaskNameRequired)
                .MaximumLength(100).WithMessage(ErrorMessages.TaskNameMaxLength);

            RuleFor(job => job.Description)
                .MaximumLength(500).WithMessage(ErrorMessages.TaskDescriptionMaxLength);
        }
    }
}
