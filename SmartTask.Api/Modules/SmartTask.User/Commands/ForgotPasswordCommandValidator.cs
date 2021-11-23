using FluentValidation;

namespace SmartTask.User.Commands
{
    class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator() : base()
        {
            RuleFor(command => command.UserName).NotNull();
        }
    }
}
