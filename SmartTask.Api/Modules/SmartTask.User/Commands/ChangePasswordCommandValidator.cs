using FluentValidation;

namespace SmartTask.User.Commands
{
    class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator() : base()
        {
            RuleFor(command => command.OldPassword).NotNull();
            RuleFor(command => command.NewPassword).NotNull();
            RuleFor(command => command.NewPasswordConfirm).NotNull();
        }
    }
}
